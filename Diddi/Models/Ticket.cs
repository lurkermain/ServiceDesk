using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Diddi.Models
{
    public enum TicketPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

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

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public byte[] File { get; set; }

        // Владелец тикета (Пользователь)
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Users? Owner { get; set; }

        // Администратор, работающий с тикетом
        public int? AdminHelperId { get; set; }
        [ForeignKey("AdminHelperId")]
        public Users? AdminHelper { get; set; }

        // Файлы, привязанные к тикету
        public List<Files> Files { get; set; } = new();
    }
}
