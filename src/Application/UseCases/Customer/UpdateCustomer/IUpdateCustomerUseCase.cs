using Application.Commons;

namespace Application.UseCases.Customer.UpdateCustomer
{
    public interface IUpdateCustomerUseCase
    {
        Task<Result<bool>> Execute(int id, UpdateCustomerInput input);
    }
}