using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TestTask66Bit.Data.Entites;

namespace TestTask66Bit.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Intern> Interns => Set<Intern>();
        public DbSet<Internship> Internships => Set<Internship>();
        public DbSet<Project> Projects => Set<Project>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Intern>().HasIndex(i => i.Email).IsUnique();
            modelBuilder.Entity<Intern>().HasIndex(i => i.Phone).IsUnique();
        }
    }
}
