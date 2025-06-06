using System.Threading;
using System.Threading.Tasks;
using Application.Commons;

namespace Application.UseCases.Sector.CreateSector
{
    public interface ICreateSectorUseCase
    {
        Task<Result<int>> ExecuteAsync(CreateSectorInput input, CancellationToken cancellationToken);
    }
}