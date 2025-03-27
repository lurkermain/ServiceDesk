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
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public byte[]? File { get; set; }

        // Владелец тикета (Обычный пользователь)
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Users? Owner { get; set; }

        // Администратор, назначенный на тикет
        public int? AdminHelperId { get; set; }
        [ForeignKey("AdminHelperId")]
        public Admin? AdminHelper { get; set; }

    }
}
