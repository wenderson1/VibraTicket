using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Customer.UpdateCustomer
{
    public class UpdateCustomerUseCase : IUpdateCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<UpdateCustomerInput> _validator;
        private readonly ILogger<UpdateCustomerUseCase> _log;

        public UpdateCustomerUseCase(
            IUnitOfWork uow,
            IValidator<UpdateCustomerInput> validator,
            ILogger<UpdateCustomerUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<bool>> Execute(int id, UpdateCustomerInput input)
        {
            try
            {
                _log.LogInformation("Iniciando atualização de Customer ID: {0}", id);

                ValidationResult validationResult = _validator.Validate(input);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Validação do UpdateCustomerInput falhou: {@ValidationErrors}", validationResult.Errors);
                    return Result.Failure<bool>(Error.FromValidationResult(validationResult));
                }

                var customer = await _uow.Customers.GetByIdAsync(id);
                if (customer is null)
                {
                    _log.LogWarning("Cliente não encontrado com ID: {0}", id);
                    return Result.Failure<bool>(Error.NotFound("Cliente não encontrado"));
                }

                if (!string.IsNullOrEmpty(input.Email) && input.Email != customer.Email)
                {
                    var existingCustomerByEmail = await _uow.Customers.GetByEmailAsync(input.Email);
                    if (existingCustomerByEmail != null && existingCustomerByEmail.Id != id)
                    {
                        _log.LogWarning("Já existe outro cliente cadastrado com o email: {0}", input.Email);
                        return Result.Failure<bool>(Error.Conflict("Já existe outro cliente cadastrado com este email"));
                    }
                }

                if (!string.IsNullOrEmpty(input.Document) && input.Document != customer.Document)
                {
                    var existingCustomerByDocument = await _uow.Customers.GetByDocumentAsync(input.Document);
                    if (existingCustomerByDocument != null && existingCustomerByDocument.Id != id)
                    {
                        _log.LogWarning("Já existe outro cliente cadastrado com o documento: {0}", input.Document);
                        return Result.Failure<bool>(Error.Conflict("Já existe outro cliente cadastrado com este documento"));
                    }
                }

                UpdateCustomerProperties(customer, input);
                _uow.Customers.Update(customer);
                await _uow.SaveChangesAsync();

                _log.LogInformation("Cliente atualizado com sucesso. ID: {0}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro inesperado durante a atualização do cliente ID: {CustomerId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }

        private static void UpdateCustomerProperties(Domain.Entities.Customer customer, UpdateCustomerInput input)
        {
            if (!string.IsNullOrEmpty(input.FullName))
                customer.FullName = input.FullName;

            if (!string.IsNullOrEmpty(input.Name))
                customer.Name = input.Name;

            if (!string.IsNullOrEmpty(input.Email))
                customer.Email = input.Email;

            if (!string.IsNullOrEmpty(input.Document))
                customer.Document = input.Document;

            customer.Phone = input.Phone;

            if (input.BirthDate.HasValue)
                customer.BirthDate = input.BirthDate.Value;

            customer.Address = input.Address;
            customer.City = input.City;
            customer.State = input.State;
            customer.ZipCode = input.ZipCode;

            if (input.IsActive.HasValue)
                customer.IsActive = input.IsActive.Value;
        }
    }
}