using Application.Commons;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Venue.DeleteVenue
{
    public class DeleteVenueUseCase(ILogger<DeleteVenueUseCase> log)
    {
        public async Task<Result<bool>> Execute(int id)
        {
            try
            {

            }
            catch (Exception ex)
            {
                log.LogError(ex, $"Erro ao deletar o Venue com ID {id} no banco de dados.", id);
                throw;
            }
        }
    }
}