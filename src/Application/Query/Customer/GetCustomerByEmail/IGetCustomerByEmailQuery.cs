using Application.Commons;

namespace Application.Query.Customer.GetCustomerByEmail
{
    public interface IGetCustomerByEmailQuery
    {
        Task<Result<GetCustomerByEmailOutput>> ExecuteAsync(string email, CancellationToken cancellationToken = default);
    }
}