using Domain.Enums;

namespace Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // GUID para ID de pagamento
    public decimal Amount { get; set; }
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? TransactionId { get; set; } // ID da transação no gateway de pagamento
    public string? GatewayResponse { get; set; } // Resposta completa do gateway (para auditoria)
    public DateTime PaymentDate { get; private set; } = DateTime.UtcNow;
    public DateTime? ProcessedDate { get; set; } // Quando o pagamento foi aprovado/recusado
    public bool IsActive { get; set; } = true;


    // Foreign Key
    public int OrderId { get; set; }

    // Navigation Property
    public Order Order { get; set; } = null!;
}