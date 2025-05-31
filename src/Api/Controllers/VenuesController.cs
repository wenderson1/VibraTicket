using Microsoft.AspNetCore.Mvc;
using Application.UseCases.Venue.CreateVenue;
using Api.Common;
using Application.Commons;
using Application.Query.Venue.GetVenueById;
using Application.UseCases.Venue.DeleteVenue;
using Application.UseCases.Venue.UpdateVenue;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class VenuesController(ICreateVenueUseCase createVenueUseCase,
                                  IGetVenueByIdQuery getVenueByIdQuery,
                                  IDeleteVenueUseCase deleteVenueUseCase,
                                  IUpdateVenueUseCase updateVenueUseCase,
                                  ILogger<VenuesController> controllerLogger,
                                  ILogger<ApiControllerBase> logger) : ApiControllerBase(logger)
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateVenue([FromBody] CreateVenueInput input, CancellationToken cancellationToken)
        {
            controllerLogger.LogInformation("Recebida requisição para criar Venue: {VenueName}", input?.Name ?? "Nome não fornecido");

            Result<int> result = await createVenueUseCase.ExecuteAsync(input!, cancellationToken);

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
            var result = await getVenueByIdQuery.ExecuteAsync(id);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }
    }
}