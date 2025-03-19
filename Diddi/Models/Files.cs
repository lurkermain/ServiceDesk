using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Diddi.Models
{
    public class Files
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public byte[] File { get; set; }

        // Привязка к тикету
        public int TicketId { get; set; }
        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }
    }
}
