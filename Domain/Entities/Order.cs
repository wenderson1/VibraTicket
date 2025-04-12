namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public required string OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal Subtotal { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public required string Status { get; set; } // Pending, Completed, Cancelled, Refunded
    public int? DiscountId { get; set; }
    public Discount Discount { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}