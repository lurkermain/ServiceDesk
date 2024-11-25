using Diddi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Diddi.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController : ControllerBase
    {
        private static List<Ticket> Tickets = new();

        [HttpGet]
        public IActionResult GetTickets()
        {
            return Ok(Tickets);
        }

        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket newTicket)
        {
            if (string.IsNullOrEmpty(newTicket.Name) || string.IsNullOrEmpty(newTicket.Description))
            {
                return BadRequest("Title and Description are required.");
            }

            Tickets.Add(newTicket);
            return Ok(newTicket);
        }

    }
}
