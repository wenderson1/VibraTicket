namespace Application.UseCases.Order.CreateOrder
{
    public class CreateOrderInput
    {
        public required int CustomerId { get; set; }
        public required List<Guid> TicketIds { get; set; }
    }
}