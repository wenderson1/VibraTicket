namespace Domain.Entities;

public class Affiliate
{
    public long Id { get; set; }
    public required string FullName { get; set; }
    public required string Name { get; set; }
    public required string Document { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string BankName { get; set; }
    public string BankAccount { get; set; }
    public string BankBranch { get; set; }
    public decimal DefaultCommissionRate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Event> Events { get; set; } = new List<Event>();
}