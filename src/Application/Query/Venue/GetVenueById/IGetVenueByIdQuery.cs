using Application.Commons;

namespace Application.Query.Venue.GetVenueById;

public interface IGetVenueByIdQuery
{
Task<Result<GetVenueByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
