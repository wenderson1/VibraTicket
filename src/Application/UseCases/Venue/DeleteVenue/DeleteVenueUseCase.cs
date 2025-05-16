using Application.Commons;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venue.DeleteVenue
{
    public class DeleteVenueUseCase(IUnitOfWork uow, ILogger<DeleteVenueUseCase> log)
    {
        public async Task<Result<bool>> Execute(int id)
        {
            try
            {
                var venue = await uow.Venues.GetByIdAsync(id);
                if (venue is null)
                {
                    log.LogWarning($"Tentativa de buscar venue pelo ID: {id} no banco de dados, mas não foi encontrado.");
                    return Result.Failure<bool>(Error.NotFound("Não foi encontrado registros!"));
                }

                uow.Venues.Delete(venue!);
                await uow.SaveChangesAsync();

                return Result.Success(true);
            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Erro ao deletar o Venue com ID {id} no banco de dados.", id);
                throw;
            }
        }
    }
}