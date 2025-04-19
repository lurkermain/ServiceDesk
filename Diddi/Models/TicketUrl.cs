using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Diddi.Models
{
    public class TicketUrl
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public string? File { get; set; }

        // Владелец тикета (Обычный пользователь)
        public int OwnerId { get; set; }

        // Администратор, назначенный на тикет
        public int? AdminHelperId { get; set; }

    }
}
