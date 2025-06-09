using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Customer.CreateCustomer
{
    public class CreateCustomerUseCase : ICreateCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateCustomerInput> _validator;
        private readonly ILogger<CreateCustomerUseCase> _log;

        public CreateCustomerUseCase(
            IUnitOfWork uow,
            IValidator<CreateCustomerInput> validator,
            ILogger<CreateCustomerUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<int>> ExecuteAsync(CreateCustomerInput input, CancellationToken cancellationToken)
        {
            try
            {
                _log.LogInformation("Iniciando criação de Customer com nome: {0}", input.Name);
                
                ValidationResult validationResult = _validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Validação do CreateCustomerInput falhou: {@ValidationErrors}", validationResult.Errors);
                    return Result.Failure<int>(Error.FromValidationResult(validationResult));
                }

                // Verificar se já existe um customer com o mesmo email
                var existingCustomerByEmail = await _uow.Customers.GetByEmailAsync(input.Email);
                if (existingCustomerByEmail != null)
                {
                    _log.LogWarning("Já existe um cliente cadastrado com o email: {0}", input.Email);
                    return Result.Failure<int>(Error.Conflict("Já existe um cliente cadastrado com este email."));
                }

                // Verificar se já existe um customer com o mesmo documento
                var existingCustomerByDocument = await _uow.Customers.GetByDocumentAsync(input.Document);
                if (existingCustomerByDocument != null)
                {
                    _log.LogWarning("Já existe um cliente cadastrado com o documento: {0}", input.Document);
                    return Result.Failure<int>(Error.Conflict("Já existe um cliente cadastrado com este documento."));
                }

                var customer = MapToCustomer(input);
                await _uow.Customers.AddAsync(customer);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Cliente criado com sucesso. ID: {0}", customer.Id);

                return Result.Success(customer.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro inesperado durante a criação do cliente {CustomerName}.", input.Name);
                throw;
            }
        }

        private static Domain.Entities.Customer MapToCustomer(CreateCustomerInput input)
        {
            return new Domain.Entities.Customer
            {
                FullName = input.FullName,
                Name = input.Name,
                Email = input.Email,
                Document = input.Document,
                Phone = input.Phone,
                BirthDate = input.BirthDate,
                Address = input.Address,
                City = input.City,
                State = input.State,
                ZipCode = input.ZipCode,
                IsActive = true
            };
        }
    }
}