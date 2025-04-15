namespace Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public required Event Event { get; set; }
        public int CustomerId { get; set; }
        public required Customer Customer { get; set; }
        public int Rating { get; set; } // 1-5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
