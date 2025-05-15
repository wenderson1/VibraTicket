using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Query.Venue.GetVenueById;

public class GetVenueByIdQuery(IUnitOfWork uow, ILogger<GetVenueByIdQuery> log) : IGetVenueByIdQuery
{
    public async Task<Result<GetVenueByIdOutput>> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var venue = await uow.Venues.GetByIdAsync(id);
            if (venue == null)
            {
                log.LogWarning($"Tentativa de buscar venue pelo ID: {id} no banco de dados, mas não foi encontrado.");
                return Result.Failure<GetVenueByIdOutput>(Error.NotFound("Não foi encontrado registros!"));           
            }

            return Result.Success(new GetVenueByIdOutput
            {
                Id = venue.Id,
                Name = venue.Name,
                Address = venue.Address,
                City = venue.City,
                State = venue.State,
                ZipCode = venue.ZipCode,
                Capacity = venue.Capacity,
            });
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Ocorreu uma excecao ao buscar venue pelo ID: {id} no banco de dados.", id);
            return Result.Failure<GetVenueByIdOutput>(Error.InternalError);
        }
    }
}
