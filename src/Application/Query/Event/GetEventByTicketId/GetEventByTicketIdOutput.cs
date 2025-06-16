using Domain.Enums;

namespace Application.Query.Event.GetEventByTicketId
{
    public class GetEventByTicketIdOutput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; }
        public string? BannerImageUrl { get; set; }
        public int MinimumAge { get; set; }
        public int VenueId { get; set; }
        public int AffiliateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property data
        public string VenueName { get; set; } = string.Empty;
        public string VenueAddress { get; set; } = string.Empty;
        public string VenueCity { get; set; } = string.Empty;
        public string VenueState { get; set; } = string.Empty;
        public string? VenueZipCode { get; set; }
        public int VenueCapacity { get; set; }

        // Ticket specific information
        public string TicketNumber { get; set; } = string.Empty;
        public string SectorName { get; set; } = string.Empty;
        public decimal TicketPrice { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public bool IsTicketUsed { get; set; }
        public DateTime? TicketUsedDate { get; set; }
    }
}