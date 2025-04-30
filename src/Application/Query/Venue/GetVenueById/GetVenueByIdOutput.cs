namespace Application.Query.Venue.GetVenueById;

public class GetVenueByIdOutput      
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public string? ZipCode { get; set; }
    public int Capacity { get; set; }
}