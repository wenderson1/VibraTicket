namespace Domain.Entities
{
    public class Sector
    {
        public int Id { get; private set; }
        public required string Name { get; set; } // Ex: "Pista", "VIP", "Camarote A"
        public decimal Price { get; set; } // Preço base do ingresso neste setor
        public int Capacity { get; set; } // Capacidade total do setor
        public int AvailableTickets { get; set; } // Quantidade disponível (pode ser calculado ou armazenado)

        // Foreign Key
        public int EventId { get; set; }

        // Navigation Property
        public Event Event { get; set; } = null!;
        public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>(); // Tickets específicos deste setor   
    }
}
