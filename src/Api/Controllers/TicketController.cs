using Microsoft.AspNetCore.Mvc;
using Api.Common;
using Application.Interfaces;
using Domain.Entities;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class TicketController : ApiControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TicketController> _controllerLogger;

        public TicketController(
            IUnitOfWork unitOfWork,
            ILogger<TicketController> controllerLogger,
            ILogger<ApiControllerBase> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
            _controllerLogger = controllerLogger;
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTicketById(Guid id, CancellationToken cancellationToken)
        {
            _controllerLogger.LogInformation("Recebida requisição para buscar Ticket ID: {TicketId}", id);

            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);

            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
    }
}
