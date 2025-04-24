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
			var tickets = await _context.Ticket
		.Select(t => new
		{
			Id = t.Id,
			Name = t.Name,
			Description = t.Description,
			Priority = t.Priority,
			Status = t.Status,
			CreatedDate = t.CreatedDate,
			UpdatedDate = t.UpdatedDate,
			OwnerId = t.OwnerId,
			AdminHelperId = t.AdminHelperId,
			ImageUrl = t.File != null ? Url.Action("GetFile", "Tickets", new { id = t.Id }) : null
		})
		.ToListAsync();

			return Ok(tickets);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTicket([FromForm] TicketCreateDto ticketDto, int ownerId, int adminId)
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
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				OwnerId = ownerId,
				AdminHelperId = adminId,
				File = await FileHelper.ConvertToByteArrayAsync(ticketDto.File),
			};

			_context.Ticket.Add(ticket);
			await _context.SaveChangesAsync();

			return Ok(ticket);
		}

		[HttpPatch]
		public async Task<IActionResult> UpdateTicket(int id, [FromForm] TicketCreateDto ticketDto, int ownerId, int adminId)
		{
			var ticket = await _context.Ticket.FindAsync(id);
			if (ticket == null)
				return NotFound();

			ticket.Name = ticketDto.Name ?? ticket.Name;
			ticket.Description = ticketDto.Description ?? ticket.Description;
			ticket.Priority = ticketDto.Priority;
			ticket.Status = ticketDto.Status;
			ticket.UpdatedDate = DateTime.Now;
			ticket.OwnerId = ownerId;
			ticket.AdminHelperId = adminId;

			if (ticketDto.File != null)
			{
				ticket.File = await FileHelper.ConvertToByteArrayAsync(ticketDto.File);
			}

			await _context.SaveChangesAsync();
			return Ok(ticket);
		}

		[HttpDelete]
		public async Task<IActionResult> DeleteTicket(int id)
		{
			var ticket = await _context.Ticket.FindAsync(id);
			if (ticket == null)
				return NotFound();

			_context.Ticket.Remove(ticket);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		[HttpGet("file")]
		public async Task<IActionResult> GetFile(int id)
		{
			var ticket = await _context.Ticket.FindAsync(id);
			if (ticket?.File == null)
				return NotFound();

			return File(ticket.File, "application/octet-stream", $"ticket_{id}.png");
		}


	}
}
