using System;

namespace Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int VenueId { get; set; }
    public Venue Venue { get; set; }
    public long AffiliateId { get; set; }
    public Affiliate Affiliate { get; set; }
    public int EventCategoryId { get; set; }
    public EventCategory Category { get; set; }
    public int TotalCapacity { get; set; }
    public int MinimumAge { get; set; }
    public required string Status { get; set; } // Draft, Published, Cancelled, Completed
    public string BannerImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CancellationPolicy { get; set; }
    public ICollection<Sector> Sectors { get; set; } = new List<Sector>();
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Customer> InterestedCustomers { get; set; } = new List<Customer>();
}