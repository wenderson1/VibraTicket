using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Event.GetEventById
{
    public class GetEventByIdQuery : IGetEventByIdQuery
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetEventByIdQuery> _log;

        public GetEventByIdQuery(IUnitOfWork uow, ILogger<GetEventByIdQuery> log)
        {
            _uow = uow;
            _log = log;
        }

        public async Task<Result<GetEventByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                _log.LogInformation("Buscando evento com ID: {0}", id);
                
                var event_ = await _uow.Events.GetByIdAsync(id);
                
                if (event_ == null)
                {
                    _log.LogWarning("Evento não encontrado com ID: {0}", id);
                    return Result.Failure<GetEventByIdOutput>(Error.NotFound("Não foi encontrado o Evento"));
                }

                var output = new GetEventByIdOutput
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
                    VenueName = event_.Venue.Name,
                    VenueAddress = event_.Venue.Address,
                    VenueCity = event_.Venue.City,
                    VenueState = event_.Venue.State,
                    VenueZipCode = event_.Venue.ZipCode,
                    VenueCapacity = event_.Venue.Capacity,
                    
                    // Affiliate data
                    AffiliateName = event_.Affiliate.Name,
                    AffiliateDocument = event_.Affiliate.Document,
                    AffiliateEmail = event_.Affiliate.Email,
                    AffiliatePhone = event_.Affiliate.Phone
                };

                _log.LogInformation("Evento encontrado com sucesso. ID: {0}", id);
                return Result.Success(output);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Erro ao buscar evento com ID: {EventId}", id);
                return Result.Failure<GetEventByIdOutput>(Error.InternalError);
            }
        }
    }
}