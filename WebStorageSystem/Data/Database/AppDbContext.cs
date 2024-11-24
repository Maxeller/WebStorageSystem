using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        // Folder: Product
        public DbSet<Bundle> Bundles { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<UnitBundleView> UnitsBundleView { get; set; } // VIEW

        // Folder: Location
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }

        // Folder: Defects
        public DbSet<Defect> Defects { get; set; }

        // Folder: Transfer
        public DbSet<MainTransfer> MainTransfers { get; set; }
        public DbSet<SubTransfer> SubTransfers { get; set; }

        // Folder: Identity
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Folder: Root
        public DbSet<ImageEntity> Images { get; set; }

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
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(manufacturer => !manufacturer.IsDeleted);
                entity.ToTable("Manufacturers");
            });
            modelBuilder.Entity<ProductType>(entity =>
            {
                entity
                    .HasMany(productType => productType.Products)
                    .WithOne(product => product.ProductType)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(productType => !productType.IsDeleted);
                entity.ToTable("ProductTypes");
            });
            modelBuilder.Entity<Vendor>(entity =>
            {
                entity
                    .HasMany(vendor => vendor.Units)
                    .WithOne(unit => unit.Vendor)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(vendor => !vendor.IsDeleted);
                entity.ToTable("Vendors");
            });
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasAlternateKey(product => product.ProductNumber);
                entity
                    .HasMany(product => product.Units)
                    .WithOne(unit => unit.Product)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(product => !product.IsDeleted);
                entity.ToTable("Products");
            });
            modelBuilder.Entity<Bundle>(entity =>
            {
                entity
                    .HasMany(bundle => bundle.BundledUnits)
                    .WithOne(unit => unit.PartOfBundle)
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(bundle => bundle.Location)
                    .WithMany(location => location.Bundles)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(bundle => bundle.DefaultLocation)
                    .WithMany(location => location.DefaultBundles)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(bundle => bundle.InventoryNumber).IsUnique();
                entity.HasQueryFilter(bundle => !bundle.IsDeleted);
                entity.ToTable("Bundles");
            });
            modelBuilder.Entity<Unit>(entity =>
            {
                entity
                    .HasOne(unit => unit.Location)
                    .WithMany(location => location.Units)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(unit => unit.DefaultLocation)
                    .WithMany(location => location.DefaultUnits)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(unit => unit.InventoryNumber).IsUnique();
                entity.HasQueryFilter(unit => !unit.IsDeleted);
                entity.ToTable("Units");
            });

            // Views as in https://learn.microsoft.com/en-us/ef/core/modeling/keyless-entity-types?tabs=data-annotations#mapping-to-database-objects
            modelBuilder.Entity<UnitBundleView>(entity =>
            {
                entity.HasKey(view => view.InventoryNumber);
                entity.ToView("UnitBundleView");
            });

            // Folder: Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity
                    .HasOne(location => location.LocationType)
                    .WithMany(locationType => locationType.Locations)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(location => !location.IsDeleted);
                entity.ToTable("Locations");
            });
            modelBuilder.Entity<LocationType>(entity =>
            {
                entity.HasQueryFilter(locationType => !locationType.IsDeleted);
                entity.ToTable("LocationTypes");
            });

            // Folder: Transfer

            modelBuilder.Entity<MainTransfer>(entity =>
            {
                entity.HasAlternateKey(mainTransfer => mainTransfer.TransferNumber);
                entity
                    .HasOne(mainTransfer => mainTransfer.User)
                    .WithMany(user => user.Transfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasMany(mainTransfer => mainTransfer.SubTransfers)
                    .WithOne(subTransfer => subTransfer.MainTransfer)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(mainTransfer => mainTransfer.DestinationLocation)
                    .WithMany(location => location.DestinationMainTransfers)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("MainTransfers");
            });

            modelBuilder.Entity<SubTransfer>(entity =>
            {
                entity
                    .HasOne(subTransfer => subTransfer.OriginLocation)
                    .WithMany(location => location.OriginTransfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(subTransfer => subTransfer.DestinationLocation)
                    .WithMany(location => location.DestinationTransfers)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(subTransfer => subTransfer.Unit)
                    .WithMany(unit => unit.SubTransfers)
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(subTransfer => subTransfer.Bundle)
                    .WithMany(bundle => bundle.SubTransfers)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.ToTable("SubTransfers");
            });

            // Folder: Defect
            modelBuilder.Entity<Defect>(entity =>
            {
                entity.HasIndex(defect => defect.DefectNumber).IsUnique();
                entity
                    .HasOne(defect => defect.Unit)
                    .WithMany(unit => unit.Defects)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(defect => defect.ReportedByUser)
                    .WithMany(user => user.ReportedDefects)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
                entity
                    .HasOne(defect => defect.CausedByUser)
                    .WithMany(user => user.CausedDefects)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.HasQueryFilter(defect => !defect.IsDeleted);
                entity.ToTable("Defects");
            });

            // Folder: Identity
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity
                    .HasMany(user => user.SubscribedLocations)
                    .WithMany(location => location.UsersSubscribed);
                entity.ToTable("Users");
            });

            // Folder: Root
            modelBuilder.Entity<ImageEntity>(entity =>
            {
                entity.HasAlternateKey(image => image.ImageName);
                entity
                    .HasMany(image => image.Products)
                    .WithOne(product => product.Image);
                entity
                    .HasMany(image => image.Defects)
                    .WithOne(defect => defect.Image);
                entity.ToTable("Images");
            });
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

        private void CheckEntries()
        {
            var entries =
                ChangeTracker.Entries()
                    .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                ((BaseEntity)entry.Entity).ModifiedDate = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Added:
                        ((BaseEntity)entry.Entity).CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        ((BaseEntity)entry.Entity).IsDeleted = true;
                        break;
                }
            }
        }
    }
}