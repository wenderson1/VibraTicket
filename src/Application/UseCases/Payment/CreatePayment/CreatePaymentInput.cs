using Domain.Enums;

namespace Application.UseCases.Payment.CreatePayment
{
    public class CreatePaymentInput
    {
        public required int OrderId { get; set; }
        public required decimal Amount { get; set; }
        public required PaymentMethod Method { get; set; }
        public string? TransactionId { get; set; }
        public string? GatewayResponse { get; set; }
    }
}