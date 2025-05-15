using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venue.CreateVenue
{
    public class CreateVenueUseCase(IUnitOfWork uow, IValidator<CreateVenueInput> validator, ILogger<CreateVenueUseCase> log) : ICreateVenueUseCase
    {
        public async Task<Result<int>> ExecuteAsync(CreateVenueInput input, CancellationToken cancellationToken)
        {
            try
            {
                log.LogInformation("Iniciando criação de Venue com nome: {0}", input.Name);
                ValidationResult validationResult = input.Validate(validator);
                if (!validationResult.IsValid)
                {
                    log.LogWarning("Validação do CreateVenueInput falhou: {@ValidationErrors}", validationResult.Errors);
                    Error appError = Error.FromValidationResult(validationResult);
                    return Result.Failure<int>(appError);
                }

                var venue = MapToVenue(input);
                await uow.Venues.AddAsync(venue);

                await uow.SaveChangesAsync(cancellationToken);

                return venue.Id;
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Erro ao salvar o Venue com nome {input.Name} no banco de dados.", input.Name);
                return Result.Failure<int>(Error.InternalError);
            }
        }

        private static Domain.Entities.Venue MapToVenue(CreateVenueInput input)
        {
            return new Domain.Entities.Venue()
            {
                Name = input.Name,
                Address = input.Address,
                City = input.City,
                State = input.State,
                ZipCode = input.ZipCode,
                Capacity = input.Capacity,
                Latitude = input.Latitude,
                Longitude = input.Longitude
            };
        }
    }
}