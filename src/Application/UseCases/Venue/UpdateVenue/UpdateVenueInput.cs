namespace Application.UseCases.Venue.UpdateVenue
{
    public record UpdateVenueInput(
        string? Name = null,
        string? Address = null,
        string? City = null,
        string? State = null,
        string? ZipCode = null,
        int? Capacity = null,
        double? Latitude = null,
        double? Longitude = null
    );
}