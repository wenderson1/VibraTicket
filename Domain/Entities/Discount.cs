namespace Domain.Entities
{
    public class Discount
    {
        public int Id { get; set; }
        public required string Code { get; set; }
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; } = 0;
        public int? EventId { get; set; }
        public required Event Event { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
    }
}
