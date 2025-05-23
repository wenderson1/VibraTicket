using Application.Commons;

namespace Application.UseCases.Venue.DeleteVenue
{
    public interface IDeleteVenueUseCase
    {
        Task<Result<bool>> Execute(int id);
    }
}