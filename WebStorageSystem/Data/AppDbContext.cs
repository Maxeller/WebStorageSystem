﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Locations;
using WebStorageSystem.Data.Entities.Products;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
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

        // Folder: Identity
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Folder: Product
            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity
                    .HasMany(manufacturer => manufacturer.Products)
                    .WithOne(product => product.Manufacturer)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(manufacturer => !manufacturer.IsDeleted);
                entity.ToTable("Manufacturers");
            });
            modelBuilder.Entity<ProductType>(entity =>
            {
                entity
                    .HasMany(productType => productType.Products)
                    .WithOne(product => product.ProductType)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(productType => !productType.IsDeleted);
                entity.ToTable("ProductTypes");
            });
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity
                    .HasMany(vendor => vendor.Units)
                    .WithOne(unit => unit.Vendor)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(vendor => !vendor.IsDeleted);
                entity.ToTable("Vendors");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                    .HasMany(product => product.Units)
                    .WithOne(unit => unit.Product)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(product => !product.IsDeleted);
                entity.ToTable("Products");
            });
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasIndex(unit => unit.SerialNumber).IsUnique();
                entity.HasQueryFilter(unit => !unit.IsDeleted);
                entity.ToTable("Units");
            });

            // Folder: Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity
                    .HasOne(location => location.LocationType)
                    .WithMany(locationType => locationType.Locations)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(location => !location.IsDeleted);
                entity.ToTable("Locations");
            });
            modelBuilder.Entity<LocationType>(entity =>
            {
                entity.HasQueryFilter(locationType => !locationType.IsDeleted);
                entity.ToTable("LocationTypes");
            });

            // Folder: Transfer
            modelBuilder.Entity<Transfer>(entity =>
            {
                entity
                    .HasOne(transfer => transfer.User)
                    .WithMany(user => user.Transfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(transfer => transfer.OriginLocation)
                    .WithMany(location => location.OriginTransfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(transfer => transfer.DestinationLocation)
                    .WithMany(location => location.DestinationTransfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(transfer => !transfer.IsDeleted);
                entity.ToTable("Transfers");
            });

            modelBuilder.Entity<TransferUnit>(entity =>
            {
                entity.HasKey(transferUnit => new {transferUnit.TransferId, transferUnit.UnitId});
                entity
                    .HasOne(transferUnit => transferUnit.Transfer)
                    .WithMany(transfer => transfer.TransferredUnits)
                    .HasForeignKey(transferUnit => transferUnit.TransferId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(transferUnit => transferUnit.Unit)
                    .WithMany(unit => unit.TransferredUnits)
                    .HasForeignKey(transferUnit => transferUnit.UnitId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(transferUnit => !transferUnit.IsDeleted);
                entity.ToTable("TransferUnits");
            });

            // Folder: Identity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });
        }

        private void CheckEntries()
        {
            var entries =
                ChangeTracker.Entries()
                    .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                ((BaseEntity) entry.Entity).ModifiedDate = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        ((BaseEntity) entry.Entity).CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        ((BaseEntity) entry.Entity).IsDeleted = true;
                        break;
                }
            }
        }
        public override int SaveChanges()
        {
            CheckEntries();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            CheckEntries();
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<WebStorageSystem.Models.LocationModels.LocationTypeModel> LocationTypeModel { get; set; }
    }
}