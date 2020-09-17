﻿using System.Net.Mime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Models.Identity;
using WebStorageSystem.Models.Location;
using WebStorageSystem.Models.Product;

namespace WebStorageSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
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
        //public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            //modelBuilder.Entity<Transfer>().ToTable("Transfers");
        }
    }
}