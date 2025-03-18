namespace Diddi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Description { get; set; }
        public DateTime? CreatedDate { get; set; } = default;
        public string? Priority { get; set; }
        public string? Owner { get; set; }
        public string? OwnerId { get; set; }
        public string? AdminHelper { get; set; }
        public bool? IsClosed { get; set; }
    }
}
