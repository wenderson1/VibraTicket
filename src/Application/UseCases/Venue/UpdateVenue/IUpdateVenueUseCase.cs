using Application.Commons;

namespace Application.UseCases.Venue.UpdateVenue;

public interface IUpdateVenueUseCase
{
    Task<Result<bool>> Execute(int id, UpdateVenueInput input);
}