using Application.Commons;

namespace Application.UseCases.Sector.UpdateSector;

public interface IUpdateSectorUseCase
{
    Task<Result<bool>> Execute(int id, UpdateSectorInput input);
}
