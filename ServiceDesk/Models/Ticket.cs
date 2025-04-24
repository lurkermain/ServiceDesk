using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Diddi.Models
{
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TicketPriority
	{
		Low,
		Medium,
		High,
		Critical
	}
	[JsonConverter(typeof(JsonStringEnumConverter))]
	public enum TicketStatus
	{
		Open,
		InProgress,
		Solved,
		Closed
	}
	public class Ticket
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;

		public DateTime CreatedDate { get; set; }
		public DateTime? UpdatedDate { get; set; }

		public TicketPriority Priority { get; set; } = TicketPriority.Medium;
		public TicketStatus Status { get; set; } = TicketStatus.Open;

		public byte[]? File { get; set; }

		public int OwnerId { get; set; }
		public int? AdminHelperId { get; set; }

	}
}
