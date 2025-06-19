using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Commons;
using Application.UseCases.Payment.CreatePayment;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class PaymentController : ApiControllerBase
    {
        private readonly ICreatePaymentUseCase _createPaymentUseCase;
        private readonly ILogger<PaymentController> _controllerLogger;

        public PaymentController(
            ICreatePaymentUseCase createPaymentUseCase,
            ILogger<PaymentController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createPaymentUseCase = createPaymentUseCase;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para criar Payment para o pedido: {OrderId}", input?.OrderId);

            Result<Guid> result = await _createPaymentUseCase.ExecuteAsync(input!, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(null, new { id = result.Value }, new { Id = result.Value });
            }
            return MapErrorToActionResult(result.Error);
        }
    }
}
