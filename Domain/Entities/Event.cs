using System;

namespace Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Address { get; set; }
    public int AffiliateId { get; set; }
    public int Capacity { get; set; }
    public int LegalAge { get; set; }
    public string Status { get; set; }
    public List<Ticket> Tickets { get; set; }

    public Event()
    {
        Tickets = new List<Ticket>();
    }
}
