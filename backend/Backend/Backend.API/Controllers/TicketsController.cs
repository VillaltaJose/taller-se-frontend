using Backend.API.Filters;
using Backend.Core.Entities.Tickets;
using Backend.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("{ticketId:int}")]
        public IActionResult GetTickets(int ticketId)
        {
            var resp = _ticketService.GetTicketById(ticketId);

            return Ok(resp);
        }

        [HttpGet("{ticketId:int}/messages")]
        public IActionResult GetMessages(int ticketId)
        {
            var resp = _ticketService.GetMessages(ticketId);

            return Ok(resp);
        }

        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket ticket)
        {
            var resp = _ticketService.CreateTicket(ticket);

            return Ok(resp);
        }

        [HttpPost("{ticketId}/messages")]
        public IActionResult AddMessage(int ticketId, [FromBody] TicketMessage message)
        {
            var resp = _ticketService.AddMessage(message);

            return Ok(resp);
        }       
    }
}
