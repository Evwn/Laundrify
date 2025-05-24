using Microsoft.EntityFrameworkCore;

namespace Laundrify.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasMaxLength(450);
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.PasswordHash).HasMaxLength(256);
                entity.Property(e => e.Role).HasMaxLength(50);
            });

            // Configure Service entity
            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasPrecision(18, 2);
            });

            // Configure Order entity
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Quantity).HasPrecision(18, 2);
                
                entity.HasOne(e => e.Client)
                    .WithMany()
                    .HasForeignKey("ClientId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Service)
                    .WithMany()
                    .HasForeignKey("ServiceId")
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
