using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Customer.GetCustomerById
{
    public class GetCustomerByIdQuery : IGetCustomerByIdQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetCustomerByIdQuery> _log;

        public GetCustomerByIdQuery(IUnitOfWork uow, ILogger<GetCustomerByIdQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetCustomerByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando customer com ID: {0}", id);
                var customer = await _uow.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    _log.LogWarning("Customer não encontrado com ID: {0}", id);
                    return Result.Failure<GetCustomerByIdOutput>(Error.NotFound("Cliente não encontrado"));
                }

                var output = new GetCustomerByIdOutput
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

                _log.LogInformation("Customer encontrado com sucesso. ID: {0}", id);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar customer com ID: {CustomerId}", id);
                return Result.Failure<GetCustomerByIdOutput>(Error.InternalError);
            }
        }
    }
}