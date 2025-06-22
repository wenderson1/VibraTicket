using Api.Common;
using Application.Commons;
using Application.Query.Customer.GetCustomerById;
using Application.Query.Customer.GetCustomerByDocument;
using Application.Query.Customer.GetCustomerByEmail;
using Application.UseCases.Customer.CreateCustomer;
using Application.UseCases.Customer.UpdateCustomer;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : ApiControllerBase
    {
        private readonly ICreateCustomerUseCase _createCustomerUseCase;
        private readonly IUpdateCustomerUseCase _updateCustomerUseCase;
        private readonly IGetCustomerByIdQuery _getCustomerByIdQuery;
        private readonly IGetCustomerByDocumentQuery _getCustomerByDocumentQuery;
        private readonly IGetCustomerByEmailQuery _getCustomerByEmailQuery;
        private readonly ILogger<CustomerController> _controllerLogger;

        public CustomerController(
            ICreateCustomerUseCase createCustomerUseCase,
            IUpdateCustomerUseCase updateCustomerUseCase,
            IGetCustomerByIdQuery getCustomerByIdQuery,
            IGetCustomerByDocumentQuery getCustomerByDocumentQuery,
            IGetCustomerByEmailQuery getCustomerByEmailQuery,
            ILogger<CustomerController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createCustomerUseCase = createCustomerUseCase;
            _updateCustomerUseCase = updateCustomerUseCase;
            _getCustomerByIdQuery = getCustomerByIdQuery;
            _getCustomerByDocumentQuery = getCustomerByDocumentQuery;
            _getCustomerByEmailQuery = getCustomerByEmailQuery;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para criar Customer: {CustomerName}", input?.Name ?? "Nome não fornecido");

            Result<int> result = await _createCustomerUseCase.ExecuteAsync(input!, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetCustomerById), new { id = result.Value }, new { Id = result.Value });
            }
            return MapErrorToActionResult(result.Error);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerInput input)
        {
            _controllerLogger.LogInformation("Recebida requisição para atualizar Customer ID: {CustomerId}", id);

            var result = await _updateCustomerUseCase.Execute(id, input);

            if (result.IsSuccess)
                return Ok(new { Success = true });

            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken = default)
        {
            _controllerLogger.LogInformation("Recebida requisição para buscar Customer ID: {CustomerId}", id);

            var result = await _getCustomerByIdQuery.ExecuteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("by-document/{document}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerByDocument(string document, CancellationToken cancellationToken = default)
        {
            _controllerLogger.LogInformation("Recebida requisição para buscar Customer por documento: {CustomerDocument}", document);

            var result = await _getCustomerByDocumentQuery.ExecuteAsync(document, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("by-email/{email}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCustomerByEmail(string email, CancellationToken cancellationToken = default)
        {
            _controllerLogger.LogInformation("Recebida requisição para buscar Customer por email: {CustomerEmail}", email);

            var result = await _getCustomerByEmailQuery.ExecuteAsync(email, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }
    }
}