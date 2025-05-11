using Diddi.Helpers;
using Diddi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

		[HttpGet("export")]
		public async Task<IActionResult> ExportTicketsToExcel()
		{
			try
			{
				var tickets = await _context.Ticket
					.OrderByDescending(x => x.UpdatedDate)
					.ThenByDescending(x => x.CreatedDate)
					.Select(t => new
					{
						t.Id,
						t.Name,
						t.Description,
						t.Priority,
						t.Status,
						t.CreatedDate,
						t.UpdatedDate,
						t.OwnerId,
						t.AdminHelperId
					})
					.ToListAsync();

				ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
				// Генерация Excel
				using (var package = new ExcelPackage(new FileInfo("MyWorkbook.xlsx")))
				{
					var worksheet = package.Workbook.Worksheets.Add("Tickets");

					// Заголовки столбцов
					worksheet.Cells[1, 1].Value = "ID";
					worksheet.Cells[1, 2].Value = "Name";
					worksheet.Cells[1, 3].Value = "Description";
					worksheet.Cells[1, 4].Value = "Priority";
					worksheet.Cells[1, 5].Value = "Status";
					worksheet.Cells[1, 6].Value = "Created Date";
					worksheet.Cells[1, 7].Value = "Updated Date";

					// Данные тикетов
					for (int i = 0; i < tickets.Count; i++)
					{
						var ticket = tickets[i];
						worksheet.Cells[i + 2, 1].Value = ticket.Id;
						worksheet.Cells[i + 2, 2].Value = ticket.Name;
						worksheet.Cells[i + 2, 3].Value = ticket.Description;
						worksheet.Cells[i + 2, 4].Value = ticket.Priority;
						worksheet.Cells[i + 2, 5].Value = ticket.Status;
						worksheet.Cells[i + 2, 6].Value = ticket.CreatedDate.ToString("yyyy-MM-dd HH:mm");
						worksheet.Cells[i + 2, 7].Value = ticket.UpdatedDate?.ToString("yyyy-MM-dd HH:mm") ?? "-";
					}

					// Устанавливаем автоширину для всех колонок
					worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

					// Создаем поток для отправки файла
					var fileStream = new MemoryStream();
					package.SaveAs(fileStream);
					fileStream.Position = 0;

					// Возвращаем файл Excel
					return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Tickets_Report.xlsx");
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.ToString(), "Error generating Excel report");
				return StatusCode(500, "Internal server error");
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetTickets()
		{
			var tickets = await _context.Ticket.OrderByDescending(x=> x.UpdatedDate).ThenByDescending(x=> x.CreatedDate)
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
