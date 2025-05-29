using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Sector.CreateSector;

public class CreateSectorUseCase : ICreateSectorUseCase
{
    private readonly IUnitOfWork uow;
    private readonly IValidator<CreateSectorInput> validator;
    private readonly ILogger<CreateSectorUseCase> log;

    public CreateSectorUseCase(IUnitOfWork uow,
                               IValidator<CreateSectorInput> validator,
                               ILogger<CreateSectorUseCase> log)
    {
        this.uow = uow;
        this.validator = validator;
        this.log = log;
    }

    public async Task<Result<int>> ExecuteAsync(CreateSectorInput input, CancellationToken cancellationToken)
    {
        try
        {
            log.LogInformation("Iniciando criação de Sector com nome: {0}", input.Name);
            ValidationResult validationResult = input.Validate(validator);
            if (!validationResult.IsValid)
            {
                log.LogWarning("Validação do CreateSectorInput falhou: {@ValidationErrors}", validationResult.Errors);
                Error appError = Error.FromValidationResult(validationResult);
                return Result.Failure<int>(appError);
            }

            var sector = MapToSector(input);
            await uow.Sectors.AddAsync(sector);
            await uow.SaveChangesAsync(cancellationToken);

            log.LogInformation("Criado Sector: {0}", input);

            return sector.Id;
        }
        catch (Exception ex)
        {
            log.LogError(ex, "Erro inesperado durante a execução de CreateSectorUseCase para o Sector com nome {SectorName}.", input.Name);
            throw;
        }
    }
    private static Domain.Entities.Sector MapToSector(CreateSectorInput input)
    {
        return new Domain.Entities.Sector()
        {
            Name = input.Name,
            Price = input.Price,
            Capacity = input.Capacity,
            AvailableTickets = input.AvailableTickets,
            EventId = input.EventId
        };
    }
}



