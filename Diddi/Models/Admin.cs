using System.ComponentModel.DataAnnotations;

namespace Diddi.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        // Тикеты, которые назначены этому админу
        public List<Ticket> AssignedTickets { get; set; } = new();
    }
}
