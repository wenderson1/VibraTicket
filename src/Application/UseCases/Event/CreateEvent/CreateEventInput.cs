namespace Application.UseCases.Event.CreateEvent
{
    public class CreateEventInput
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? BannerImageUrl { get; set; }
        public int MinimumAge { get; set; }
        public int VenueId { get; set; }
        public int AffiliateId { get; set; }
    }
}