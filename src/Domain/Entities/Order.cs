using Domain.Enums;

namespace Domain.Entities;

public class Order
{
    public int Id { get; private set; }
    public required string OrderNumber { get; set; } // Número único do pedido
    public decimal TotalAmount { get; set; } // Valor total do pedido
    public OrderStatus Status { get; set; } = OrderStatus.PendingPayment;
    public DateTime OrderDate { get; private set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    // Foreign Key
    public int CustomerId { get; set; }

    // Navigation Properties
    public Customer Customer { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; private set; } = new List<Ticket>(); // Tickets incluídos neste pedido
    public ICollection<Payment> Payments { get; private set; } = new List<Payment>(); // Pagamentos associados
}