using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Customer.GetCustomerByDocument
{
    public class GetCustomerByDocumentQuery : IGetCustomerByDocumentQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetCustomerByDocumentQuery> _log;

        public GetCustomerByDocumentQuery(IUnitOfWork uow, ILogger<GetCustomerByDocumentQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetCustomerByDocumentOutput>> ExecuteAsync(string document, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando customer com documento: {0}", document);
                var customer = await _uow.Customers.GetByDocumentAsync(document);
                if (customer == null)
                {
                    _log.LogWarning("Customer não encontrado com documento: {0}", document);
                    return Result.Failure<GetCustomerByDocumentOutput>(Error.NotFound("Cliente não encontrado"));
                }

                var output = new GetCustomerByDocumentOutput
                {
                    Id = customer.Id,
                    FullName = customer.FullName,
                    Name = customer.Name,
                    Email = customer.Email,
                    Document = customer.Document,
                    Phone = customer.Phone,
                    BirthDate = customer.BirthDate,
                    Address = customer.Address,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    CreatedAt = customer.CreatedAt,
                    IsActive = customer.IsActive
                };

                _log.LogInformation("Customer encontrado com sucesso. Documento: {0}", document);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar customer com documento: {CustomerDocument}", document);
                return Result.Failure<GetCustomerByDocumentOutput>(Error.InternalError);
            }
        }
    }
}