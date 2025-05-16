using System.Reflection.Metadata;
using Domain.Entities;
using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;

namespace Application.UseCases.Venue.UpdateVenue;

public class UpdateVenueUseCase(IUnitOfWork uow, IValidator<UpdateVenueInput> validator, ILogger<UpdateVenueUseCase> log)
{
    public async Task<Result<bool>> Execute(int id, UpdateVenueInput input)
    {
        try
        {
            log.LogInformation("Iniciando processo de criação de Venue para: {VenueName}", input.Name);
            ValidationResult validationResult = validator.Validate(input);
            if (!validationResult.IsValid)
            {
                log.LogWarning($"Validação do UpdateVenueInput falhou: [{0}]", validationResult.Errors);
                return Result.Failure<bool>(Error.FromValidationResult(validationResult));
            }

            var venue = await uow.Venues.GetByIdAsync(id);
            if (venue is null)
            {
                return Result.Failure<bool>(Error.NotFound("Não foi encontrado o Local"));
            }

            uow.Venues.Update(MapToVenue(input, venue));
            await uow.SaveChangesAsync();
            return Result.Success<bool>(true);
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Erro ao salvar o Venue com nome {input.Name} no banco de dados.", input.Name);
            return Result.Failure<bool>(Error.InternalError);
        }
    }

    private Domain.Entities.Venue MapToVenue(UpdateVenueInput input, Domain.Entities.Venue venue)
    {
        return new Domain.Entities.Venue
        {
            Name = input.Name ?? venue.Name,
            Address = input.Address ?? venue.Address,
            City = input.City ?? venue.City,
            State = input.State ?? venue.State,
            ZipCode = input.ZipCode ?? venue.ZipCode,
            Capacity = input.Capacity ?? venue.Capacity,
            Latitude = input.Latitude ?? venue.Latitude,
            Longitude = input.Longitude ?? venue.Longitude
        };
    }
}