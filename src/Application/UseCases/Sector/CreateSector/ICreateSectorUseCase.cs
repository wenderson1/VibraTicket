using System.Threading;
using System.Threading.Tasks;

namespace Application.UseCases.Sector.CreateSector
{
    public interface ICreateSectorUseCase
    {
        public Task<Result<int>> ExecuteAsync(CreateSectorInput input, CancellationToken cancellationToken);
    }
}