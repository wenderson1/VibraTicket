using Application.Commons;

namespace Application.Query.Customer.GetCustomerByDocument
{
    public interface IGetCustomerByDocumentQuery
    {
        Task<Result<GetCustomerByDocumentOutput>> ExecuteAsync(string document, CancellationToken cancellationToken = default);
    }
}