using Domain.Enums;

namespace Application.UseCases.Order.UpdateOrder
{
    public class UpdateOrderInput
    {
        public OrderStatus? Status { get; set; }
        public bool? IsActive { get; set; }
        // Não permitimos atualizar CustomerId ou TotalAmount diretamente
        // pois isso quebraria a integridade dos dados
    }
}