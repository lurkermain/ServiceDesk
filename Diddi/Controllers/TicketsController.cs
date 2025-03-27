using Diddi.Helpers;
using Diddi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
        public async Task<IActionResult> CreateTicket([FromForm] TicketCreateDto ticketDto)
        {
            if (string.IsNullOrEmpty(ticketDto.Name) || string.IsNullOrEmpty(ticketDto.Description))
            {
                return BadRequest(new { message = "Title and Description are required." });
            }

            var ticket = new Ticket
            {
                Name = ticketDto.Name,
                Description = ticketDto.Description,
                Priority = ticketDto.Priority,
                Status = ticketDto.Status,
                CreatedDate = DateTime.UtcNow,
                File = await FileHelper.ConvertToByteArrayAsync(ticketDto.File),
            };

            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }


    }
}
