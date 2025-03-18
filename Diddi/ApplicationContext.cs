using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using Diddi.Models;

namespace Diddi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Images> Images { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Ticket> Ticket { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
/*            Database.EnsureDeleted();
            Database.EnsureCreated();*/
        }
    }
}
