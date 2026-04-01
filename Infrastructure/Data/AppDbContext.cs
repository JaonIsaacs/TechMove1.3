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
        public DbSet<FileMetadata> FileMetadatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔹 CLIENT
            modelBuilder.Entity<Client>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Client>()
                .Property(c => c.Region)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Client>()
                .HasIndex(c => c.Name);

            // 🔹 CONTRACT
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Contracts)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contract>()
                .Property(c => c.StartDate)
                .IsRequired();

            modelBuilder.Entity<Contract>()
                .Property(c => c.EndDate)
                .IsRequired();

            modelBuilder.Entity<Contract>()
                .Property(c => c.SignedFilePath)
                .HasMaxLength(500);

            modelBuilder.Entity<Contract>()
                .HasIndex(c => c.StartDate);

            // 🔹 SERVICE REQUEST
            modelBuilder.Entity<ServiceRequest>()
                .HasOne(sr => sr.Contract)
                .WithMany(c => c.ServiceRequests)
                .HasForeignKey(sr => sr.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ServiceRequest>()
                .Property(sr => sr.Description)
                .HasMaxLength(500);

            // 🔹 FILE METADATA
            modelBuilder.Entity<FileMetadata>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<FileMetadata>()
                .HasOne(f => f.Contract)
                .WithMany(c => c.Files)
                .HasForeignKey(f => f.ContractId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FileMetadata>()
                .Property(f => f.FileName)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<FileMetadata>()
                .Property(f => f.Path)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<FileMetadata>()
                .Property(f => f.ContentType)
                .HasMaxLength(100);

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