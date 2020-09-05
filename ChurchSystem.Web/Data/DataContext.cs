using ChurchSystem.Common.Entities;
using ChurchSystem.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChurchSystem.Web.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Church> Churches { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Profession> Professions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Church>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Field>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<District>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Profession>()
                .HasIndex(t => t.Name)
                .IsUnique();
        }
    }
}
