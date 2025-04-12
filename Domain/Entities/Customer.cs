using System;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Gender { get; set; }
    public required string Document { get; set; }
    public required string Email { get; set; }
    public string Phone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Event> FavoriteEvents { get; set; } = new List<Event>();
}