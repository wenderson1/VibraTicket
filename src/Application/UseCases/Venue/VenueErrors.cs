using Application.Commons;

namespace Application.UseCases.Venue;

public static class VenueErrors
{
    public static Error CreationFailed = Error.Failure(
        "Venue.CreationFailed",
        "Não foi possível salvar o local no banco de dados.");
    public static Error InternalError = Error.Failure(
        "Venue.InternalError",
        "Não foi possível completar a sua requisicão.");
    public static Error NotFound(int id) => Error.NotFound(
        "Venue.NotFound",
        $"O local com o ID {id} não foi encontrado.");
}
