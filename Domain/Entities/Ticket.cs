namespace Domain.Entities;

public class Ticket
{
    public int Id { get; set; }
    public required string TicketNumber { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public int SectorId { get; set; }
    public Sector Sector { get; set; }
    public int? CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int? OrderId { get; set; }
    public Order Order { get; set; }
    public decimal Price { get; set; }
    public decimal Tax { get; set; }
    public decimal Commission { get; set; }
    public required string Status { get; set; } // Available, Reserved, Sold, Used, Cancelled
    public bool IsUsed { get; set; }
    public DateTime? UsedDate { get; set; }
    public required string QrCode { get; set; }
    public int QrCodeVersion { get; set; } = 1;
    public required string Type { get; set; } // Regular, VIP, Student, etc.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime ValidUntil { get; set; }
}