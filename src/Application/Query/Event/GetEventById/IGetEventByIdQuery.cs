using Application.Commons;

namespace Application.Query.Event.GetEventById
{
    public interface IGetEventByIdQuery
    {
        Task<Result<GetEventByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
    }
}