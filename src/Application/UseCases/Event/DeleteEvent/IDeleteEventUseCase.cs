using Application.Commons;

namespace Application.UseCases.Event.DeleteEvent
{
    public interface IDeleteEventUseCase
    {
        Task<Result<bool>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
    }
}