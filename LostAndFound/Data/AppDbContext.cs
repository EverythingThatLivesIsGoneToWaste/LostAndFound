using LostAndFound.Models;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Building> Buildings => Set<Building>();
        public DbSet<FoundItem> FoundItems => Set<FoundItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FullName).IsRequired().HasMaxLength(200);

                entity.Property(e => e.Password).IsRequired().HasMaxLength(100);

                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).HasConversion<string>();
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

                entity.Property(e => e.Description).HasMaxLength(150);
            });

            modelBuilder.Entity<FoundItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);

                entity.Property(e => e.Info).IsRequired().HasMaxLength(150);

                entity.Property(e => e.FoundAt).IsRequired();
            });

            modelBuilder.Entity<Building>()
                .HasMany(e => e.FoundItems)
                .WithOne(e => e.Building)
                .HasForeignKey(e => e.BuildingId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
