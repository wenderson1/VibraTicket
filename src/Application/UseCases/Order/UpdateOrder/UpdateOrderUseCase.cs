using Application.Commons;
using Application.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Order.UpdateOrder
{
    public class UpdateOrderUseCase : IUpdateOrderUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<UpdateOrderInput> _validator;
        private readonly ILogger<UpdateOrderUseCase> _log;

        public UpdateOrderUseCase(
            IUnitOfWork uow,
            IValidator<UpdateOrderInput> validator,
            ILogger<UpdateOrderUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<bool>> ExecuteAsync(int id, UpdateOrderInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando atualiza��o do pedido ID: {0}", id);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Valida��o falhou para atualiza��o do pedido. Erros: {@ValidationErrors}", 
                        validationResult.Errors);
                    return Result.Failure<bool>(Error.FromValidationResult(validationResult));
                }

                var order = await _uow.Orders.GetByIdAsync(id);
                if (order == null)
                {
                    _log.LogWarning("Pedido n�o encontrado com ID: {0}", id);
                    return Result.Failure<bool>(Error.NotFound("Pedido n�o encontrado"));
                }

                // Validar as transi��es de status permitidas
                if (input.Status.HasValue && !IsValidStatusTransition(order.Status, input.Status.Value))
                {
                    _log.LogWarning("Transi��o de status inv�lida de {0} para {1}", order.Status, input.Status.Value);
                    return Result.Failure<bool>(Error.Validation("Transi��o de status inv�lida"));
                }

                if (input.Status.HasValue)
                {
                    // Se estiver mudando para Completed, verificar se tem pagamento aprovado
                    if (input.Status.Value == OrderStatus.Completed)
                    {
                        var hasApprovedPayment = await HasApprovedPaymentAsync(order.Id);
                        if (!hasApprovedPayment)
                        {
                            _log.LogWarning("Tentativa de completar pedido sem pagamento aprovado. ID: {0}", id);
                            return Result.Failure<bool>(Error.Validation(
                                "N�o � poss�vel completar um pedido sem pagamento aprovado"));
                        }
                    }

                    order.Status = input.Status.Value;
                }

                if (input.IsActive.HasValue)
                {
                    // Se estiver tentando inativar um pedido que j� est� completo
                    if (!input.IsActive.Value && order.Status == OrderStatus.Completed)
                    {
                        _log.LogWarning("Tentativa de inativar pedido completo. ID: {0}", id);
                        return Result.Failure<bool>(Error.Validation(
                            "N�o � poss�vel inativar um pedido que j� foi completado"));
                    }

                    order.IsActive = input.IsActive.Value;
                }

                _uow.Orders.Update(order);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Pedido atualizado com sucesso. ID: {0}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao atualizar pedido ID: {OrderId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }

        private async Task<bool> HasApprovedPaymentAsync(int orderId)
        {
            var payments = await _uow.Payments.GetByOrderIdAsync(orderId);
            return payments.Any(p => p.Status == PaymentStatus.Approved && p.IsActive);
        }

        private static bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                // Transi��es permitidas
                (OrderStatus.PendingPayment, OrderStatus.Completed) => true,
                (OrderStatus.PendingPayment, OrderStatus.Cancelled) => true,
                (OrderStatus.Completed, OrderStatus.Refunded) => true,
                // Mesmo status � permitido
                var (current, new_) when current == new_ => true,
                // Qualquer outra transi��o � inv�lida
                _ => false
            };
        }
    }
}