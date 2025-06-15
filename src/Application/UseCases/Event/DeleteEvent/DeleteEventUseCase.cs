using Application.Commons;
using Application.Interfaces;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Event.DeleteEvent
{
    public class DeleteEventUseCase : IDeleteEventUseCase
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteEventUseCase> _log;

        public DeleteEventUseCase(IUnitOfWork uow, ILogger<DeleteEventUseCase> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<bool>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Iniciando exclus�o do evento ID: {EventId}", id);

                var event_ = await _uow.Events.GetByIdAsync(id);
                if (event_ == null)
                {
                    _log.LogWarning("Evento n�o encontrado com ID: {EventId}", id);
                    return Result.Failure<bool>(Error.NotFound("Evento n�o encontrado"));
                }

                // N�o permitir excluir eventos com status Published ou Completed
                if (event_.Status is EventStatus.Published or EventStatus.Completed)
                {
                    _log.LogWarning("N�o � poss�vel excluir evento com status {Status}. ID: {EventId}", 
                        event_.Status, id);
                    return Result.Failure<bool>(Error.Validation(
                        "N�o � poss�vel excluir um evento que j� foi publicado ou finalizado"));
                }

                // Verificar se existem tickets associados
                var hasTickets = await _uow.Events.HasTicketsAsync(id);
                if (hasTickets)
                {
                    _log.LogWarning("N�o � poss�vel excluir evento com tickets associados. ID: {EventId}", id);
                    return Result.Failure<bool>(Error.Validation(
                        "N�o � poss�vel excluir um evento que possui tickets associados"));
                }

                _uow.Events.Delete(event_);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Evento exclu�do com sucesso. ID: {EventId}", id);
                return Result.Success(true);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao excluir evento ID: {EventId}", id);
                return Result.Failure<bool>(Error.InternalError);
            }
        }
    }
}