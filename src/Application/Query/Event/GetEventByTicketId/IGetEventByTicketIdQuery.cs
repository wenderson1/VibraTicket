using Application.Commons;

namespace Application.Query.Event.GetEventByTicketId
{
    public interface IGetEventByTicketIdQuery
    {
        Task<Result<GetEventByTicketIdOutput>> ExecuteAsync(Guid ticketId, CancellationToken cancellationToken = default);
    }
}