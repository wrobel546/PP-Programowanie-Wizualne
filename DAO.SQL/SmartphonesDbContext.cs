using System;
using Microsoft.EntityFrameworkCore;

namespace NowakowskaWrobel.Smartphones.DAO.SQL
{
    public class SmartphonesDbContext : DbContext
    {
        public DbSet<ProducerSqlDO> Producers { get; set; } = null!;
        public DbSet<SmartphoneSqlDO> Smartphones { get; set; } = null!;

        private readonly string _connectionString;

        public SmartphonesDbContext()
        {
            var folder = AppContext.BaseDirectory;
            var dbPath = System.IO.Path.Combine(folder, "smartphones.db");
            _connectionString = $"Data Source={dbPath}";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProducerSqlDO>(entity =>
            {
                entity.ToTable("Producers");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Country).HasMaxLength(100);
            });

            modelBuilder.Entity<SmartphoneSqlDO>(entity =>
            {
                entity.ToTable("Smartphones");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.ModelName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Price).HasColumnType("decimal(18,2)");

                entity
                    .HasOne(s => s.ProducerNavigation)
                    .WithMany(p => p.Smartphones)
                    .HasForeignKey(s => s.ProducerId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
