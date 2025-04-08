namespace Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public int CustomerId { get; set; }
    public decimal Tax { get; set; }
    public decimal Commission { get; set; }
    public decimal Price { get; set; }
    public bool Used { get; set; }
    public required string QrCode { get; set; }
    public required string Type { get; set; }
}
