using Application.Commons;
using Application.Interfaces;
using Domain.Enums;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Event.CreateEvent
{
    public class CreateEventUseCase : ICreateEventUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<CreateEventInput> _validator;
        private readonly ILogger<CreateEventUseCase> _log;

        public CreateEventUseCase(
            IUnitOfWork uow,
            IValidator<CreateEventInput> validator,
            ILogger<CreateEventUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<int>> ExecuteAsync(CreateEventInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando criação de evento: {EventName}", input.Name);

                // Validação do input
                var validationResult = await _validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Validação falhou para evento: {EventName}. Erros: {@Errors}", 
                        input.Name, validationResult.Errors);
                    return Result.Failure<int>(Error.FromValidationResult(validationResult));
                }

                // Validação do Venue
                var venue = await _uow.Venues.GetByIdAsync(input.VenueId);
                if (venue == null)
                {
                    _log.LogWarning("Local não encontrado com ID: {VenueId}", input.VenueId);
                    return Result.Failure<int>(Error.NotFound("Local do evento não encontrado"));
                }

                // Validação do Affiliate
                var affiliate = await _uow.Affiliates.GetByIdAsync(input.AffiliateId);
                if (affiliate == null)
                {
                    _log.LogWarning("Afiliado não encontrado com ID: {AffiliateId}", input.AffiliateId);
                    return Result.Failure<int>(Error.NotFound("Afiliado não encontrado"));
                }

                if (!affiliate.IsActive)
                {
                    _log.LogWarning("Afiliado está inativo. ID: {AffiliateId}", input.AffiliateId);
                    return Result.Failure<int>(Error.Validation("Não é possível criar evento com um afiliado inativo"));
                }

                // Verificar se já existe evento no mesmo local e período
                var conflictingEvent = await _uow.Events.GetByVenueAndDateRangeAsync(
                    input.VenueId,
                    input.StartDate,
                    input.EndDate);

                if (conflictingEvent != null)
                {
                    _log.LogWarning("Já existe um evento no local {VenueId} no período especificado", input.VenueId);
                    return Result.Failure<int>(Error.Validation("Já existe um evento neste local e período"));
                }

                var newEvent = new Domain.Entities.Event
                {
                    Name = input.Name,
                    Description = input.Description,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    Status = EventStatus.Draft,
                    BannerImageUrl = input.BannerImageUrl,
                    MinimumAge = input.MinimumAge,
                    VenueId = input.VenueId,
                    AffiliateId = input.AffiliateId
                };

                await _uow.Events.AddAsync(newEvent);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Evento criado com sucesso. ID: {EventId}, Nome: {EventName}", 
                    newEvent.Id, newEvent.Name);

                return Result.Success(newEvent.Id);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao criar evento: {EventName}", input.Name);
                return Result.Failure<int>(Error.InternalError);
            }
        }
    }
}