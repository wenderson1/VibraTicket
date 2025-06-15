using Application.Commons;
using Application.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Event.UpdateEvent
{
    public class UpdateEventUseCase : IUpdateEventUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<UpdateEventInput> _validator;
        private readonly ILogger<UpdateEventUseCase> _log;

        public UpdateEventUseCase(
            IUnitOfWork uow,
            IValidator<UpdateEventInput> validator,
            ILogger<UpdateEventUseCase> log)
        {
            _uow = uow;
            _validator = validator;
            _log = log;
        }

        public async Task<Result<bool>> ExecuteAsync(int id, UpdateEventInput input, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando atualização do evento ID: {EventId}", id);

                var validationResult = await _validator.ValidateAsync(input, cancellationToken);
                if (!validationResult.IsValid)
                {
                    _log.LogWarning("Validação falhou para atualização do evento ID: {EventId}. Erros: {@Errors}", 
                        id, validationResult.Errors);
                    return Result.Failure<bool>(Error.FromValidationResult(validationResult));
                }

                var event_ = await _uow.Events.GetByIdAsync(id);
                if (event_ == null)
                {
                    _log.LogWarning("Evento não encontrado com ID: {EventId}", id);
                    return Result.Failure<bool>(Error.NotFound("Evento não encontrado"));
                }

                if (input.VenueId.HasValue)
                {
                    var venue = await _uow.Venues.GetByIdAsync(input.VenueId.Value);
                    if (venue == null)
                    {
                        _log.LogWarning("Local não encontrado com ID: {VenueId}", input.VenueId.Value);
                        return Result.Failure<bool>(Error.NotFound("Local do evento não encontrado"));
                    }
                }

                if (input.AffiliateId.HasValue)
                {
                    var affiliate = await _uow.Affiliates.GetByIdAsync(input.AffiliateId.Value);
                    if (affiliate == null)
                    {
                        _log.LogWarning("Afiliado não encontrado com ID: {AffiliateId}", input.AffiliateId.Value);
                        return Result.Failure<bool>(Error.NotFound("Afiliado não encontrado"));
                    }

                    if (!affiliate.IsActive)
                    {
                        _log.LogWarning("Afiliado está inativo. ID: {AffiliateId}", input.AffiliateId.Value);
                        return Result.Failure<bool>(Error.Validation("Não é possível atualizar evento com um afiliado inativo"));
                    }
                }

                // Verificar conflito de datas apenas se houver mudança nas datas ou local
                if ((input.StartDate.HasValue || input.EndDate.HasValue || input.VenueId.HasValue) &&
                    await HasDateConflict(event_, input))
                {
                    _log.LogWarning("Conflito de datas detectado para o evento ID: {EventId}", id);
                    return Result.Failure<bool>(Error.Validation("Já existe um evento neste local e período"));
                }

                UpdateEventProperties(event_, input);

                event_.UpdatedAt = DateTime.UtcNow;
                _uow.Events.Update(event_);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Evento atualizado com sucesso. ID: {EventId}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao atualizar evento ID: {EventId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }

        private async Task<bool> HasDateConflict(Domain.Entities.Event currentEvent, UpdateEventInput input)
        {
            var startDate = input.StartDate ?? currentEvent.StartDate;
            var endDate = input.EndDate ?? currentEvent.EndDate;
            var venueId = input.VenueId ?? currentEvent.VenueId;

            var conflictingEvent = await _uow.Events.GetByVenueAndDateRangeAsync(
                venueId,
                startDate,
                endDate);

            return conflictingEvent != null && conflictingEvent.Id != currentEvent.Id;
        }

        private void UpdateEventProperties(Domain.Entities.Event event_, UpdateEventInput input)
        {
            if (input.Name != null)
                event_.Name = input.Name;
            
            if (input.Description != null)
                event_.Description = input.Description;
            
            if (input.StartDate.HasValue)
                event_.StartDate = input.StartDate.Value;
            
            if (input.EndDate.HasValue)
                event_.EndDate = input.EndDate.Value;
            
            if (input.Status.HasValue)
                event_.Status = input.Status.Value;
            
            if (input.BannerImageUrl != null)
                event_.BannerImageUrl = input.BannerImageUrl;
            
            if (input.MinimumAge.HasValue)
                event_.MinimumAge = input.MinimumAge.Value;
            
            if (input.VenueId.HasValue)
                event_.VenueId = input.VenueId.Value;
            
            if (input.AffiliateId.HasValue)
                event_.AffiliateId = input.AffiliateId.Value;
        }
    }
}