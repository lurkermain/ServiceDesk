using Diddi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diddi.Controllers
{
    [ApiController]
    [Route("api/tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public TicketsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            return Ok(await _context.Ticket.ToListAsync());
        }

        [HttpPost]
        public IActionResult CreateTicket([FromForm] Ticket newTicket)
        {
            if (string.IsNullOrEmpty(newTicket.Name) || string.IsNullOrEmpty(newTicket.Description))
            {
                return BadRequest("Title and Description are required.");
            }

            _context.Ticket.Add(newTicket);
            return Ok(newTicket);
        }

    }
}
