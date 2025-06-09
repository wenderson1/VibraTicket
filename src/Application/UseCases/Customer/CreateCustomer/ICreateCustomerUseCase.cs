using Application.Commons;

namespace Application.UseCases.Customer.CreateCustomer
{
    public interface ICreateCustomerUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateCustomerInput input, CancellationToken cancellationToken);
    }
}