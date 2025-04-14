using Domain.Enums;

namespace Domain.Entities;

public class Ticket
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // Usar GUID para ID de ticket é bom para evitar sequenciais previsíveis
    public required string TicketNumber { get; set; } // Um número/código legível, talvez único por evento
    public decimal Price { get; set; } // Preço final pago (pode incluir taxas)
    public TicketStatus Status { get; set; } = TicketStatus.Available;
    public string? QrCodeData { get; set; } // Dados para gerar o QR Code
    public bool IsUsed { get; private set; } = false;
    public DateTime? UsedDate { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    // Foreign Keys
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int? CustomerId { get; set; } // Pode ser nulo se o ticket ainda não foi atribuído/vendido
    public int? OrderId { get; set; } // Pode ser nulo se o ticket não faz parte de uma ordem (ex: cortesia)

    // Navigation Properties
    public Event Event { get; set; } = null!;
    public Sector Sector { get; set; } = null!;
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }

    // Método de exemplo para lógica de domínio
    public void MarkAsUsed()
    {
        if (Status == TicketStatus.Sold && !IsUsed)
        {
            IsUsed = true;
            UsedDate = DateTime.UtcNow;
            Status = TicketStatus.Used;
        }
        // Adicionar lógica de erro/exceção se necessário
    }
}