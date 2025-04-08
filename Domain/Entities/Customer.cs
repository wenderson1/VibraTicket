using System;

namespace Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Gender { get; set; }
    public required string Document { get; set; }
    public DateTime BirthDate { get; set; }
    public List<Ticket> Tickets { get; set; }

    public Customer()
    {
        Tickets = new List<Ticket>();
    }
}
