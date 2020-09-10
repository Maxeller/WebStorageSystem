using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Location;
using WebStorageSystem.Models.Product;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem
{
    public class StorageDbContext : DbContext
    {
        public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
        {
        }

        // Folder: Product
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Vendor> Vendors { get; set; }

        // Folder: Location
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }

        // Folder: Transfer
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Folder: Product
            modelBuilder.Entity<Manufacturer>().ToTable("Manufacturers");
            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<ProductType>().ToTable("ProductTypes");
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasIndex(e => e.SerialNumber).IsUnique();
                entity.ToTable("Units");
            });
            modelBuilder.Entity<Vendor>().ToTable("Vendors");

            // Folder: Location
            modelBuilder.Entity<Location>().ToTable("Locations");
            modelBuilder.Entity<LocationType>().ToTable("LocationTypes");

            // Folder: Transfer
            modelBuilder.Entity<Transfer>().ToTable("Transfers");
        }
    }
}