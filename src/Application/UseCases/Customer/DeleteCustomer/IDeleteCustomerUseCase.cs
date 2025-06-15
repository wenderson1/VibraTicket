using Application.Commons;

namespace Application.UseCases.Customer.DeleteCustomer
{
    public interface IDeleteCustomerUseCase
    {
        Task<Result<bool>> Execute(int id);
    }
}