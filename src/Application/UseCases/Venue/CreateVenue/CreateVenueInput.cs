using Application.Commons;

namespace Application.UseCases.Venue.CreateVenue;

public record CreateVenueInput(
string Name,
string Address,
string City,
string State,
int Capacity,
string? ZipCode = null,
double? Latitude = null,
double? Longitude = null) : InputBase<CreateVenueInput>
{ }

