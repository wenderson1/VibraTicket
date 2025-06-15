using Application.Commons;

namespace Application.UseCases.Event.UpdateEvent
{
    public interface IUpdateEventUseCase
    {
        Task<Result<bool>> ExecuteAsync(int id, UpdateEventInput input, CancellationToken cancellationToken = default);
    }
}