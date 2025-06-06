using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;

namespace Application.UseCases.Sector.UpdateSector;

public class UpdateSectorUseCase(IUnitOfWork uow,
                                  IValidator<UpdateSectorInput> validator,
                                  ILogger<UpdateSectorUseCase> log) : IUpdateSectorUseCase
{
    public async Task<Result<bool>> Execute(int id, UpdateSectorInput input)
    {
        try
        {
            log.LogInformation("Iniciando processo de atualização de Sector para: {SectorId}", id);
            ValidationResult validationResult = validator.Validate(input);
            if (!validationResult.IsValid)
            {
                log.LogWarning($"Validação do UpdateSectorInput falhou: [{0}]", validationResult.Errors);
                return Result.Failure<bool>(Error.FromValidationResult(validationResult));
            }

            var sector = await uow.Sectors.GetByIdAsync(id);
            if (sector is null)
                return Result.Failure<bool>(Error.NotFound("Não foi encontrado o Setor"));

            MapToSector(input, sector);
            uow.Sectors.Update(sector);
            await uow.SaveChangesAsync();
            return Result.Success<bool>(true);
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Erro ao atualizar o Sector com id {id} no banco de dados.", id);
            return Result.Failure<bool>(Error.InternalError);
        }
    }

    private void MapToSector(UpdateSectorInput input, Domain.Entities.Sector sector)
    {
        if (input.Name is not null)
            sector.Name = input.Name;
        if (input.Capacity is not null)
            sector.Capacity = input.Capacity.Value;
        if (input.Price is not null)
            sector.Price = input.Price.Value;
        if (input.EventId is not null)
            sector.EventId = input.EventId.Value;
        if (input.AvailableTickets is not null)
            sector.AvailableTickets = input.AvailableTickets.Value;
    }
}
