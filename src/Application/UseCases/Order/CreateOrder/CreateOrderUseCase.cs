using Application.Commons;
using Application.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Order.CreateOrder
{
    public class CreateOrderUseCase : ICreateOrderUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateOrderInput> _validator;
        private readonly ILogger<CreateOrderUseCase> _log;

        public CreateOrderUseCase(
            IUnitOfWork uow,
            IValidator<CreateOrderInput> validator,
            ILogger<CreateOrderUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<int>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando criação de pedido para o cliente ID: {0}", input.CustomerId);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Validação falhou para criação do pedido. Erros: {@ValidationErrors}", 
                        validationResult.Errors);
                    return Result.Failure<int>(Error.FromValidationResult(validationResult));
                }

                // Verificar se o cliente existe
                var customer = await _uow.Customers.GetByIdAsync(input.CustomerId);
                if (customer == null)
                {
                    _log.LogWarning("Cliente não encontrado com ID: {0}", input.CustomerId);
                    return Result.Failure<int>(Error.NotFound("Cliente não encontrado"));
                }

                if (!customer.IsActive)
                {
                    _log.LogWarning("Cliente está inativo. ID: {0}", input.CustomerId);
                    return Result.Failure<int>(Error.Validation("Não é possível criar pedido para um cliente inativo"));
                }

                // Buscar e validar os tickets
                decimal totalAmount = 0;
                var tickets = new List<Domain.Entities.Ticket>();
                foreach (var ticketId in input.TicketIds)
                {
                    var ticket = await _uow.Tickets.GetByIdAsync(ticketId);
                    if (ticket == null)
                    {
                        _log.LogWarning("Ticket não encontrado com ID: {0}", ticketId);
                        return Result.Failure<int>(Error.NotFound($"Ingresso não encontrado: {ticketId}"));
                    }

                    if (ticket.Status != TicketStatus.Available)
                    {
                        _log.LogWarning("Ticket não está disponível. ID: {0}, Status: {1}", ticketId, ticket.Status);
                        return Result.Failure<int>(Error.Validation($"Ingresso não está disponível: {ticketId}"));
                    }

                    tickets.Add(ticket);
                    totalAmount += ticket.Price;
                }

                var order = new Domain.Entities.Order
                {
                    OrderNumber = GenerateOrderNumber(),
                    TotalAmount = totalAmount,
                    CustomerId = input.CustomerId,
                    Status = OrderStatus.PendingPayment
                };

                await _uow.Orders.AddAsync(order);

                // Atualizar os tickets com o OrderId e mudar o status
                foreach (var ticket in tickets)
                {
                    ticket.OrderId = order.Id;
                    ticket.Status = TicketStatus.Reserved;
                    _uow.Tickets.Update(ticket);
                }

                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Pedido criado com sucesso. ID: {0}, Total: {1}", order.Id, totalAmount);
                return Result.Success(order.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao criar pedido para o cliente ID: {CustomerId}", input.CustomerId);
                return Result.Failure<int>(Error.InternalError);
            }
        }

        private static string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        }
    }
}