using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections;
using Diddi.Models;

namespace Diddi
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Images> Images { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
