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
			modelBuilder.Entity<Ticket>()
				.Property(t => t.CreatedDate)
				.HasColumnType("timestamp without time zone");

			modelBuilder.Entity<Ticket>()
				.Property(t => t.UpdatedDate)
				.HasColumnType("timestamp without time zone");
		}
    }
}
