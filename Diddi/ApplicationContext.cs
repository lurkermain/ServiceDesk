using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using Diddi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Diddi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Files> Files { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
/*            Database.EnsureDeleted();*/
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Опционально: добавляем связи и ограничения
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // Не удалять пользователя при удалении тикета

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AdminHelper)
                .WithMany()
                .HasForeignKey(t => t.AdminHelperId)
                .OnDelete(DeleteBehavior.SetNull); // Если админ удалён, тикет остаётся

            modelBuilder.Entity<Files>()
                .HasOne(i => i.Ticket)
                .WithMany(t => t.Files)
                .HasForeignKey(i => i.TicketId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении тикета удаляются все его файлы
        }
    }
}
