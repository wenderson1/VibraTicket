using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Commons;
using Application.UseCases.Event.CreateEvent;
using Application.UseCases.Event.UpdateEvent;
using Application.UseCases.Event.DeleteEvent;
using Application.Query.Event.GetEventById;
using Application.Query.Event.GetEventByTicketId;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class EventController : ApiControllerBase
    {
        private readonly ICreateEventUseCase _createEventUseCase;
        private readonly IUpdateEventUseCase _updateEventUseCase;
        private readonly IDeleteEventUseCase _deleteEventUseCase;
        private readonly IGetEventByIdQuery _getEventByIdQuery;
        private readonly IGetEventByTicketIdQuery _getEventByTicketIdQuery;
        private readonly ILogger<EventController> _controllerLogger;

        public EventController(
            ICreateEventUseCase createEventUseCase,
            IUpdateEventUseCase updateEventUseCase,
            IDeleteEventUseCase deleteEventUseCase,
            IGetEventByIdQuery getEventByIdQuery,
            IGetEventByTicketIdQuery getEventByTicketIdQuery,
            ILogger<EventController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _createEventUseCase = createEventUseCase;
            _updateEventUseCase = updateEventUseCase;
            _deleteEventUseCase = deleteEventUseCase;
            _getEventByIdQuery = getEventByIdQuery;
            _getEventByTicketIdQuery = getEventByTicketIdQuery;
            _controllerLogger = controllerLogger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventInput input, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Received request to create Event: {EventName}", input?.Name ?? "Name not provided");

            Result<int> result = await _createEventUseCase.ExecuteAsync(input!, cancellationToken);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetEventById), new { id = result.Value }, new { Id = result.Value });
            }
            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventById(int id, CancellationToken cancellationToken)
        {
            var result = await _getEventByIdQuery.ExecuteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }

        [HttpGet("ticket/{ticketId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEventByTicketId(Guid ticketId, CancellationToken cancellationToken)
        {
            var result = await _getEventByTicketIdQuery.ExecuteAsync(ticketId, cancellationToken);

            if (result.IsSuccess)
                return Ok(result.Value);

            return MapErrorToActionResult(result.Error);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventInput input, CancellationToken cancellationToken)
        {
            var result = await _updateEventUseCase.ExecuteAsync(id, input, cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return MapErrorToActionResult(result.Error);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEvent(int id, CancellationToken cancellationToken)
        {
            var result = await _deleteEventUseCase.ExecuteAsync(id, cancellationToken);

            if (result.IsSuccess)
                return Ok();

            return MapErrorToActionResult(result.Error);
        }
    }
}