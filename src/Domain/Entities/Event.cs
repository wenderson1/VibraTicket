using Domain.Enums;

namespace Domain.Entities;

public class Event
{
    public int Id { get; private set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public EventStatus Status { get; set; } = EventStatus.Draft;
    public string? BannerImageUrl { get; set; }
    public int MinimumAge { get; set; } = 0; // Idade mínima (0 para livre)

    // Foreign Keys
    public int VenueId { get; set; }
    public int AffiliateId { get; set; } // Deve corresponder ao tipo do Affiliate.Id

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public Venue Venue { get; set; } = null!; // Marcar como não nulo com ! ou tornar nullable
    public Affiliate Affiliate { get; set; } = null!;
    public ICollection<Sector> Sectors { get; private set; } = new List<Sector>();
    public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>(); // Todos os tickets do evento
}