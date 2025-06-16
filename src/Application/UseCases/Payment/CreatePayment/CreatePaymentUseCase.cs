using Application.Commons;
using Application.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Payment.CreatePayment
{
    public class CreatePaymentUseCase : ICreatePaymentUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreatePaymentInput> _validator;
        private readonly ILogger<CreatePaymentUseCase> _log;

        public CreatePaymentUseCase(
            IUnitOfWork uow,
            IValidator<CreatePaymentInput> validator,
            ILogger<CreatePaymentUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<Guid>> ExecuteAsync(CreatePaymentInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando cria��o de pagamento para o pedido ID: {0}", input.OrderId);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Valida��o falhou para cria��o do pagamento. Erros: {@ValidationErrors}", 
                        validationResult.Errors);
                    return Result.Failure<Guid>(Error.FromValidationResult(validationResult));
                }

                // Verificar se o pedido existe
                var order = await _uow.Orders.GetByIdAsync(input.OrderId);
                if (order == null)
                {
                    _log.LogWarning("Pedido n�o encontrado com ID: {0}", input.OrderId);
                    return Result.Failure<Guid>(Error.NotFound("Pedido n�o encontrado"));
                }

                // Verificar se o pedido est� ativo
                if (!order.IsActive)
                {
                    _log.LogWarning("Pedido est� inativo. ID: {0}", input.OrderId);
                    return Result.Failure<Guid>(Error.Validation("N�o � poss�vel criar pagamento para um pedido inativo"));
                }

                // Verificar se o valor do pagamento corresponde ao valor do pedido
                if (input.Amount != order.TotalAmount)
                {
                    _log.LogWarning("Valor do pagamento ({0}) n�o corresponde ao valor do pedido ({1})", 
                        input.Amount, order.TotalAmount);
                    return Result.Failure<Guid>(Error.Validation("O valor do pagamento n�o corresponde ao valor do pedido"));
                }

                // Verificar se o pedido j� est� conclu�do ou tem outro status que n�o permite pagamento
                if (order.Status != OrderStatus.PendingPayment)
                {
                    _log.LogWarning("Pedido n�o est� pendente de pagamento. ID: {0}, Status: {1}", input.OrderId, order.Status);
                    return Result.Failure<Guid>(Error.Validation("N�o � poss�vel criar pagamento para um pedido que n�o est� pendente"));
                }

                var payment = new Domain.Entities.Payment
                {
                    OrderId = input.OrderId,
                    Amount = input.Amount,
                    Method = input.Method,
                    TransactionId = input.TransactionId,
                    GatewayResponse = input.GatewayResponse,
                    Status = PaymentStatus.Pending
                };

                await _uow.Payments.AddAsync(payment);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Pagamento criado com sucesso. ID: {0}, Pedido ID: {1}", 
                    payment.Id, payment.OrderId);
                
                return Result.Success(payment.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao criar pagamento para o pedido ID: {OrderId}", input.OrderId);
                return Result.Failure<Guid>(Error.InternalError);
            }
        }
    }
}