namespace Domain.Entities;

public class Affiliate
{
    public long Id { get; set; }
    public required string FullName { get; set; }
    public required string Name { get; set; }
    public required string Document { get; set; }
    public IEnumerable<Event> Events { get; set; } = [];
}
