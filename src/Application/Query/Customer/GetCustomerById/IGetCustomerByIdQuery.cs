using Application.Commons;

namespace Application.Query.Customer.GetCustomerById
{
    public interface IGetCustomerByIdQuery
    {
        Task<Result<GetCustomerByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default);
    }
}