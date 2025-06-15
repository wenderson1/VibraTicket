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
                _log.LogInformation("Iniciando exclusão do evento ID: {EventId}", id);

                var event_ = await _uow.Events.GetByIdAsync(id);
                if (event_ == null)
                {
                    _log.LogWarning("Evento não encontrado com ID: {EventId}", id);
                    return Result.Failure<bool>(Error.NotFound("Evento não encontrado"));
                }

                // Não permitir excluir eventos com status Published ou Completed
                if (event_.Status is EventStatus.Published or EventStatus.Completed)
                {
                    _log.LogWarning("Não é possível excluir evento com status {Status}. ID: {EventId}", 
                        event_.Status, id);
                    return Result.Failure<bool>(Error.Validation(
                        "Não é possível excluir um evento que já foi publicado ou finalizado"));
                }

                // Verificar se existem tickets associados
                var hasTickets = await _uow.Events.HasTicketsAsync(id);
                if (hasTickets)
                {
                    _log.LogWarning("Não é possível excluir evento com tickets associados. ID: {EventId}", id);
                    return Result.Failure<bool>(Error.Validation(
                        "Não é possível excluir um evento que possui tickets associados"));
                }

                _uow.Events.Delete(event_);
                await _uow.SaveChangesAsync(cancellationToken);

                _log.LogInformation("Evento excluído com sucesso. ID: {EventId}", id);
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