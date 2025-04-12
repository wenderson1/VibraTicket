namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public decimal Amount { get; set; }
    public required string Method { get; set; } // CreditCard, DebitCard, PIX, BankTransfer
    public required string Status { get; set; } // Pending, Approved, Declined, Refunded
    public required string TransactionId { get; set; }
    public string GatewayResponse { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    public int? Installments { get; set; }
    public DateTime? RefundDate { get; set; }
    public decimal? RefundAmount { get; set; }
    public string RefundReason { get; set; }
}