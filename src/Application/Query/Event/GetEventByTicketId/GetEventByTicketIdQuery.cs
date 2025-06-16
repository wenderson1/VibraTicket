using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Event.GetEventByTicketId
{
    public class GetEventByTicketIdQuery : IGetEventByTicketIdQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetEventByTicketIdQuery> _log;

        public GetEventByTicketIdQuery(IUnitOfWork uow, ILogger<GetEventByTicketIdQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetEventByTicketIdOutput>> ExecuteAsync(Guid ticketId, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando evento pelo Ticket ID: {0}", ticketId);
                
                var ticket = await _uow.Tickets.GetByIdAsync(ticketId);
                
                if (ticket == null)
                {
                    _log.LogWarning("Ticket não encontrado com ID: {0}", ticketId);
                    return Result.Failure<GetEventByTicketIdOutput>(Error.NotFound("Não foi encontrado o Ticket"));
                }

                var event_ = ticket.Event;
                var venue = event_.Venue;
                var sector = ticket.Sector;

                var output = new GetEventByTicketIdOutput
                {
                    Id = event_.Id,
                    Name = event_.Name,
                    Description = event_.Description,
                    StartDate = event_.StartDate,
                    EndDate = event_.EndDate,
                    Status = event_.Status,
                    BannerImageUrl = event_.BannerImageUrl,
                    MinimumAge = event_.MinimumAge,
                    VenueId = event_.VenueId,
                    AffiliateId = event_.AffiliateId,
                    CreatedAt = event_.CreatedAt,
                    UpdatedAt = event_.UpdatedAt,
                    
                    // Venue data
                    VenueName = venue.Name,
                    VenueAddress = venue.Address,
                    VenueCity = venue.City,
                    VenueState = venue.State,
                    VenueZipCode = venue.ZipCode,
                    VenueCapacity = venue.Capacity,
                    
                    // Ticket specific data
                    TicketNumber = ticket.TicketNumber,
                    SectorName = sector.Name,
                    TicketPrice = ticket.Price,
                    TicketStatus = ticket.Status,
                    IsTicketUsed = ticket.IsUsed,
                    TicketUsedDate = ticket.UsedDate
                };

                _log.LogInformation("Evento encontrado com sucesso através do Ticket ID: {0}", ticketId);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar evento pelo Ticket ID: {TicketId}", ticketId);
                return Result.Failure<GetEventByTicketIdOutput>(Error.InternalError);
            }
        }
    }
}