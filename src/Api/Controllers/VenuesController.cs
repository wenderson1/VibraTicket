using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Venue.CreateVenue;
using Api.Common;
using Application.Commons;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class VenuesController : ApiControllerBase
    {
        private readonly ICreateVenueUseCase _createVenueUseCase;
        private readonly ILogger<VenuesController> _controllerLogger;

        public VenuesController(
            ICreateVenueUseCase createVenueUseCase,
            ILogger<VenuesController> controllerLogger, 
            ILogger<ApiControllerBase> baseLogger)
            : base(baseLogger)
        {
            _createVenueUseCase = createVenueUseCase;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVenue([FromBody] CreateVenueInput input, CancellationToken cancellationToken)
        {
             _controllerLogger.LogInformation("Recebida requisição para criar Venue: {VenueName}", input?.Name ?? "Nome não fornecido");

            Result<int> result = await _createVenueUseCase.ExecuteAsync(input!, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetVenueById), new { id = result.Value }, new { Id = result.Value });
            }
            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVenueById(int id)
        {
             _controllerLogger.LogInformation("Requisição recebida para GetVenueById com ID: {VenueId}", id);
            await Task.Delay(10);
            return Ok($"GetVenueById chamado com ID = {id}");
        }
    }
}