namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int TicketId { get; set; }
    public decimal Amount { get; set; }
    public required string Method { get; set; }
    public required string Status { get; set; }
    public required string TransactionId { get; set; }
}
