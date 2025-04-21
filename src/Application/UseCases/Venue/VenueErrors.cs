using Application.Commons;

namespace Application.UseCases.Venue;

public static class VenueErrors
{
    public static Error CreationFailed = Error.Failure("Venue.CreationFailed","Não foi possível salvar o local no banco de dados.");
    public static Error NotFound = Error.Failure("Venue.NotFound", "O local não foi encontrado.");
}
