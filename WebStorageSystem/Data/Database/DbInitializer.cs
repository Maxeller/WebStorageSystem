using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Entities;

namespace WebStorageSystem.Data.Database
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, IConfiguration configuration)
        {
            await context.Database.MigrateAsync();

            if (configuration["SeedDatabase"] == "0") return;

            if (context.Manufacturers.Any()) return;

            var manufacturers = new[]
            {
                new Manufacturer { Name = "Sony" },
                new Manufacturer { Name = "Panasonic" }
            };
            await context.Manufacturers.AddRangeAsync(manufacturers);
            await context.SaveChangesAsync();

            var productTypes = new[]
            {
                new ProductType { Name = "Camera" },
                new ProductType { Name = "Microphone" }
            };
            await context.ProductTypes.AddRangeAsync(productTypes);
            await context.SaveChangesAsync();

            var vendors = new[]
            {
                new Vendor
                {
                    Name = "Vendor 1", Address = "Prague", Email = "worker@vendor1.com", Website = "vendor1.com",
                    Phone = "+420606120120"
                },
                new Vendor
                {
                    Name = "Vendor 2", Address = "Dublin", Email = "problems@vendor2.com", Website = "vendor2.com",
                    Phone = "+421555555555"
                }
            };
            await context.Vendors.AddRangeAsync(vendors);
            await context.SaveChangesAsync();

            var products = new[]
            {
                new Product
                {
                    Name = "Cam 3000", ProductNumber = "SC3000", Manufacturer = manufacturers[0],
                    ProductType = productTypes[0]
                },
                new Product
                {
                    Name = "Cam 5000", ProductNumber = "SC5000", Manufacturer = manufacturers[0],
                    ProductType = productTypes[0]
                },
                new Product
                {
                    Name = "SuperMic 2", ProductNumber = "PSM2", Manufacturer = manufacturers[1],
                    ProductType = productTypes[1]
                },
                new Product
                {
                    Name = "SuperMic 3", ProductNumber = "PSM3", Manufacturer = manufacturers[1],
                    ProductType = productTypes[1]
                },
            };
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();

            var locationTypes = new[]
            {
                new LocationType { Name = "Warehouse" },
                new LocationType { Name = "Truck" },
            };
            await context.LocationTypes.AddRangeAsync(locationTypes);
            await context.SaveChangesAsync();

            var locations = new[]
            {
                new Location { Name = "Main Warehouse", LocationType = locationTypes[0] },
                new Location { Name = "Secondary Warehouse", LocationType = locationTypes[0] },
                new Location { Name = "Truck 1", LocationType = locationTypes[1] },
                new Location { Name = "SNG 1", LocationType = locationTypes[1] }
            };
            await context.Locations.AddRangeAsync(locations);
            await context.SaveChangesAsync();

            var units = new[]
            {
                new Unit
                {
                    Product = products[0], InventoryNumber = "3000-1", SerialNumber = "bqKQ73cD",
                    Location = locations[0], DefaultLocation = locations[0], Vendor = vendors[0]
                },
                new Unit
                {
                    Product = products[0], InventoryNumber = "3000-2", SerialNumber = "6CqncYDV",
                    Location = locations[0], DefaultLocation = locations[0], Vendor = vendors[0]
                },
                new Unit
                {
                    Product = products[0], InventoryNumber = "3000-3", SerialNumber = "NkLdtVFr",
                    Location = locations[1], DefaultLocation = locations[0], Vendor = vendors[0]
                },
                new Unit
                {
                    Product = products[0], InventoryNumber = "3000-4", SerialNumber = "86nsRZ7k",
                    Location = locations[2], DefaultLocation = locations[0], Vendor = vendors[0]
                },
                new Unit
                {
                    Product = products[1], InventoryNumber = "5000-1", SerialNumber = "tHdSwPvK",
                    Location = locations[2], DefaultLocation = locations[0], Vendor = vendors[1]
                },
                new Unit
                {
                    Product = products[1], InventoryNumber = "5000-2", SerialNumber = "UWpY8yQ5",
                    Location = locations[2], DefaultLocation = locations[0], Vendor = vendors[1]
                },
                new Unit
                {
                    Product = products[2], InventoryNumber = "PSM2-1", SerialNumber = "VXwwReY9",
                    Location = locations[1], DefaultLocation = locations[0], Vendor = vendors[1]
                },
                new Unit
                {
                    Product = products[2], InventoryNumber = "PSM2-2", SerialNumber = "anRtD7eU",
                    Location = locations[2], DefaultLocation = locations[0], Vendor = vendors[1]
                }
            };
            await context.Units.AddRangeAsync(units);
            await context.SaveChangesAsync();

            var bundledUnits = new List<Unit>() { units[4], units[5] };
            var bundle = new Bundle
            {
                Name = "Bundle Cam 5000", InventoryNumber = "B5000", BundledUnits = bundledUnits,
                Location = locations[2], DefaultLocation = locations[0]
            };
            await context.Bundles.AddAsync(bundle);
            await context.SaveChangesAsync();
        }
    }
}