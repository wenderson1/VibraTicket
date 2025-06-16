using Application.Commons;

namespace Application.UseCases.Order.CreateOrder
{
    public interface ICreateOrderUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateOrderInput input, CancellationToken cancellationToken = default);
    }
}