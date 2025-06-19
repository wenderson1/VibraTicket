using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Commons;
using Application.UseCases.Order.CreateOrder;
using Application.UseCases.Order.UpdateOrder;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ApiControllerBase
    {
        private readonly ICreateOrderUseCase _createOrderUseCase;
        private readonly IUpdateOrderUseCase _updateOrderUseCase;
        private readonly ILogger<OrderController> _controllerLogger;

        public OrderController(
            ICreateOrderUseCase createOrderUseCase,
            IUpdateOrderUseCase updateOrderUseCase,
            ILogger<OrderController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createOrderUseCase = createOrderUseCase;
            _updateOrderUseCase = updateOrderUseCase;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para criar Order para o cliente: {CustomerId}", input?.CustomerId);

            Result<int> result = await _createOrderUseCase.ExecuteAsync(input!, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(null, new { id = result.Value }, new { Id = result.Value });
            }
            return MapErrorToActionResult(result.Error);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para atualizar Order ID: {OrderId}", id);

            var result = await _updateOrderUseCase.ExecuteAsync(id, input, cancellationToken);

            if (result.IsSuccess)
                return Ok(new { Success = true });

            return MapErrorToActionResult(result.Error);
        }
    }
}
