using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Customer.DeleteCustomer
{
    public class DeleteCustomerUseCase : IDeleteCustomerUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteCustomerUseCase> _log;

        public DeleteCustomerUseCase(IUnitOfWork uow, ILogger<DeleteCustomerUseCase> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<bool>> Execute(int id)
        {
            try
            {
                _log.LogInformation("Tentando deletar (soft delete) customer com ID: {0}", id);
                var customer = await _uow.Customers.GetByIdAsync(id);
                
                if (customer is null)
                {
                    _log.LogWarning("Customer não encontrado com ID: {0}", id);
                    return Result.Failure<bool>(Error.NotFound("Não foi encontrado o Cliente"));
                }

                customer.IsActive = false;
                _uow.Customers.Update(customer);
                await _uow.SaveChangesAsync();

                _log.LogInformation("Customer marcado como inativo com sucesso. ID: {0}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao deletar customer com ID: {CustomerId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }
    }
}