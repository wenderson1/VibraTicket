using Application.Commons;

namespace Application.UseCases.Payment.CreatePayment
{
    public interface ICreatePaymentUseCase
    {
        Task<Result<Guid>> ExecuteAsync(CreatePaymentInput input, CancellationToken cancellationToken = default);
    }
}