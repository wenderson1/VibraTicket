using Application.Commons;

namespace Application.UseCases.Venue.CreateVenue
{
    public interface ICreateVenueUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateVenueInput input, CancellationToken cancellationToken);
    }
}
