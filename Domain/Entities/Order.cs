namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int PaymentId { get; set; }
    public int TicketId { get; set; }
    public decimal Amount { get; set; }
    public int CustomerId { get; set; }
}
