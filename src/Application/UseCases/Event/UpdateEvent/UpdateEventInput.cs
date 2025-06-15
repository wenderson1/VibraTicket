using Domain.Enums;

namespace Application.UseCases.Event.UpdateEvent
{
    public class UpdateEventInput
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public EventStatus? Status { get; set; }
        public string? BannerImageUrl { get; set; }
        public int? MinimumAge { get; set; }
        public int? VenueId { get; set; }
        public int? AffiliateId { get; set; }
    }
}