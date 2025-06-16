using Application.Commons;

namespace Application.UseCases.Order.UpdateOrder
{
    public interface IUpdateOrderUseCase
    {
        Task<Result<bool>> ExecuteAsync(int id, UpdateOrderInput input, CancellationToken cancellationToken = default);
    }
}