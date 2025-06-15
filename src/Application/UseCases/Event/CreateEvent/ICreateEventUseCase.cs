using Application.Commons;

namespace Application.UseCases.Event.CreateEvent
{
    public interface ICreateEventUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateEventInput input, CancellationToken cancellationToken = default);
    }
}