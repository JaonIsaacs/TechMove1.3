using Microsoft.EntityFrameworkCore;
using TechMove1._3.Domain.Entities;

namespace TechMove1._3.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 CLIENT
            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired();

            modelBuilder.Entity<Client>()
                .Property(c => c.Region)
                .IsRequired();

            // 🔹 CONTRACT
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Contracts)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔹 SERVICE REQUEST
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Contract)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            // 🔥 IMPORTANT: Fix decimal precision (prevents data loss)
            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.CostZAR)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.CostUSD)
                .HasPrecision(18, 2);
        }
    }
}