using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ตารางเดิม
        public DbSet<Product> Products => Set<Product>();

        // ตารางใหม่
        public DbSet<Users> Users => Set<Users>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Description).HasMaxLength(1000);
                entity.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });

            // Users
            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(u => u.Password)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(u => u.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(u => u.Role)
                      .HasMaxLength(50);
                entity.Property(u => u.ContactInfo)
                      .HasMaxLength(255);
            });
        }
    }
}