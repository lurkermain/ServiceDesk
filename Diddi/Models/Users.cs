﻿using System.ComponentModel.DataAnnotations;

namespace Diddi.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Тикеты, созданные этим пользователем
        public List<Ticket> Tickets { get; set; } = new();
    }
}
