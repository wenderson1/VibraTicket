using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Customer.GetCustomerByEmail
{
    public class GetCustomerByEmailQuery : IGetCustomerByEmailQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetCustomerByEmailQuery> _log;

        public GetCustomerByEmailQuery(IUnitOfWork uow, ILogger<GetCustomerByEmailQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetCustomerByEmailOutput>> ExecuteAsync(string email, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando customer com email: {0}", email);
                var customer = await _uow.Customers.GetByEmailAsync(email);
                if (customer == null)
                {
                    _log.LogWarning("Customer não encontrado com email: {0}", email);
                    return Result.Failure<GetCustomerByEmailOutput>(Error.NotFound("Cliente não encontrado"));
                }

                var output = new GetCustomerByEmailOutput
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

                _log.LogInformation("Customer encontrado com sucesso. Email: {0}", email);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar customer com email: {CustomerEmail}", email);
                return Result.Failure<GetCustomerByEmailOutput>(Error.InternalError);
            }
        }
    }
}