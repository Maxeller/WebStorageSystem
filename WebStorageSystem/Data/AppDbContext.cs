using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Identity;
using WebStorageSystem.Models.Location;
using WebStorageSystem.Models.Product;
using WebStorageSystem.Models.Transfer;

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
                entity.ToTable("Manufacturers");
            });
            modelBuilder.Entity<ProductType>(entity =>
            {
                entity
                    .HasMany(productType => productType.Products)
                    .WithOne(product => product.ProductType)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("ProductTypes");
            });
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity
                    .HasMany(vendor => vendor.Units)
                    .WithOne(unit => unit.Vendor)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("Vendors");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity
                    .HasMany(product => product.Units)
                    .WithOne(unit => unit.Product)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("Products");
            });
            modelBuilder.Entity<Unit>(entity =>
            {
                entity.HasIndex(unit => unit.SerialNumber).IsUnique();
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
                entity.ToTable("Locations");
            });
            modelBuilder.Entity<LocationType>(entity =>
            {
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
                entity.ToTable("TransferUnits");
            });

            // Folder: Identity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
            });
        }

        public override int SaveChanges()
        {
            var entries =
                ChangeTracker.Entries()
                    .Where(e => e.Entity is BaseModel && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel) entityEntry.Entity).ModifiedDate = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedDate = DateTime.UtcNow;
                }
            }
            return base.SaveChanges();
        }
    }
}