namespace Domain.Entities
{
    public class EventCategory
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
