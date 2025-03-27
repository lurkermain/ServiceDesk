using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using Diddi.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Diddi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
/*            Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Владелец тикета (Обычный пользователь)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Owner)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.OwnerId)
                .OnDelete(DeleteBehavior.Restrict); // Не удаляем пользователя, если удалён тикет

            // Администратор тикета (Админ)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AdminHelper)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AdminHelperId)
                .OnDelete(DeleteBehavior.SetNull); // Если админ удалён, тикет остаётся

            // Убеждаемся, что администратором может быть только IsAdmin = true
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.AdminHelper)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AdminHelperId)
                .HasConstraintName("FK_AdminMustBeAdmin")
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
