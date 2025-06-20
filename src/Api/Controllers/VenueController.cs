using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Commons;
using Application.UseCases.Venue.CreateVenue;
using Application.UseCases.Venue.UpdateVenue;
using Application.UseCases.Venue.DeleteVenue;
using Application.Query.Venue.GetVenueById;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class VenueController : ApiControllerBase
    {
        private readonly ICreateVenueUseCase _createVenueUseCase;
        private readonly IUpdateVenueUseCase _updateVenueUseCase;
        private readonly IDeleteVenueUseCase _deleteVenueUseCase;
        private readonly IGetVenueByIdQuery _getVenueByIdQuery;
        private readonly ILogger<VenueController> _controllerLogger;

        public VenueController(
            ICreateVenueUseCase createVenueUseCase,
            IUpdateVenueUseCase updateVenueUseCase,
            IDeleteVenueUseCase deleteVenueUseCase,
            IGetVenueByIdQuery getVenueByIdQuery,
            ILogger<VenueController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createVenueUseCase = createVenueUseCase;
            _updateVenueUseCase = updateVenueUseCase;
            _deleteVenueUseCase = deleteVenueUseCase;
            _getVenueByIdQuery = getVenueByIdQuery;
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
        public async Task<IActionResult> GetVenueById(int id, CancellationToken cancellationToken)
        {
            var result = await _getVenueByIdQuery.ExecuteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateVenue(int id, [FromBody] UpdateVenueInput input)
        {
            _controllerLogger.LogInformation("Recebida requisição para atualizar Venue ID: {VenueId}", id);

            var result = await _updateVenueUseCase.Execute(id, input);

            if (result.IsSuccess)
                return Ok(new { Success = true });

            return MapErrorToActionResult(result.Error);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteVenue(int id)
        {
            _controllerLogger.LogInformation("Recebida requisição para deletar Venue ID: {VenueId}", id);

            var result = await _deleteVenueUseCase.Execute(id);

            if (result.IsSuccess)
                return Ok(new { Success = true });

            return MapErrorToActionResult(result.Error);
        }
    }
}
