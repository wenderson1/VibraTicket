using Domain.Enums;

namespace Application.Query.Event.GetEventById
{
    public class GetEventByIdOutput
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

        public string AffiliateName { get; set; } = string.Empty;
        public string AffiliateDocument { get; set; } = string.Empty;
        public string? AffiliateEmail { get; set; }
        public string? AffiliatePhone { get; set; }
    }
}