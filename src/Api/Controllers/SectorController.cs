using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Commons;
using Application.UseCases.Sector.CreateSector;
using Application.UseCases.Sector.UpdateSector;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class SectorController : ApiControllerBase
    {
        private readonly ICreateSectorUseCase _createSectorUseCase;
        private readonly IUpdateSectorUseCase _updateSectorUseCase;
        private readonly ILogger<SectorController> _controllerLogger;

        public SectorController(
            ICreateSectorUseCase createSectorUseCase,
            IUpdateSectorUseCase updateSectorUseCase,
            ILogger<SectorController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createSectorUseCase = createSectorUseCase;
            _updateSectorUseCase = updateSectorUseCase;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSector([FromBody] CreateSectorInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para criar Sector: {SectorName}", input?.Name);

            Result<int> result = await _createSectorUseCase.ExecuteAsync(input!, cancellationToken);

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
        public async Task<IActionResult> UpdateSector(int id, [FromBody] UpdateSectorInput input)
        {
            _controllerLogger.LogInformation("Recebida requisição para atualizar Sector ID: {SectorId}", id);

            var result = await _updateSectorUseCase.Execute(id, input);

            if (result.IsSuccess)
                return Ok(new { Success = true });

            return MapErrorToActionResult(result.Error);
        }
    }
}
