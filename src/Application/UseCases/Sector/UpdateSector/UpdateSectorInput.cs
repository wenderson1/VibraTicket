namespace Application.UseCases.Sector.UpdateSector
{
    public record UpdateSectorInput(
        string? Name = null,
        int? Capacity = null,
        decimal? Price = null,
        int? EventId = null,
        int? AvailableTickets = null
    );
}
