namespace Application.UseCases.Sector.CreateSector
{
    public class CreateSectorInput
    {
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public decimal Price { get; set; }
        public int EventId { get; set; }
        public int AvailableTickets { get; set; }
    }
}