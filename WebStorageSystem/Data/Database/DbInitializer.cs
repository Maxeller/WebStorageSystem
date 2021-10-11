using System.Linq;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Data.Database
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if(context.Products.Any()) return;

            var manufacturers = new[]
            {
                new Manufacturer {Name = "Sony"},
                new Manufacturer {Name = "Panasonic"}
            };
            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();

            var productTypes = new[]
            {
                new ProductType {Name = "Camera"},
                new ProductType {Name = "Microphone"}
            };
            context.ProductTypes.AddRange(productTypes);
            context.SaveChanges();

            var vendors = new[]
            {
                new Vendor {Name = "Vendor 1", Address = "Prague", Email = "worker@vendor1.com", Phone = "+420606120120"},
                new Vendor {Name = "Vendor 2", Address = "Dublin", Email = "problems@vendor2.com", Phone = "+421555555555"}
            };
            context.Vendors.AddRange(vendors);
            context.SaveChanges();

            var products = new[]
            {
                new Product {Name = "Cam 3000", Manufacturer = manufacturers[0], ProductType = productTypes[0]},
                new Product {Name = "Cam 5000", Manufacturer = manufacturers[0], ProductType = productTypes[0]},
                new Product {Name = "SuperMic 2", Manufacturer = manufacturers[1], ProductType = productTypes[1]},
                new Product {Name = "SuperMic 3", Manufacturer = manufacturers[1], ProductType = productTypes[1]},
            };
            context.Products.AddRange(products);
            context.SaveChanges();

            var locationTypes = new[]
            {
                new LocationType { Name = "Storage"},
                new LocationType { Name = "Car"},
            };
            context.LocationTypes.AddRange(locationTypes);
            context.SaveChanges();

            var locations = new[]
            {
                new Location {Name = "Main Storage", LocationType = locationTypes[0]},
                new Location {Name = "Car 1", LocationType = locationTypes[1]},
                new Location {Name = "Car 2", LocationType = locationTypes[1]}
            };
            context.Locations.AddRange(locations);
            context.SaveChanges();

            var units = new[]
            {
                new Unit {Product = products[0], InventoryNumber = "3000-1", Location = locations[0], Vendor = vendors[0]},
                new Unit {Product = products[0], InventoryNumber = "3000-2", Location = locations[0], Vendor = vendors[0]},
                new Unit {Product = products[0], InventoryNumber = "3000-3", Location = locations[1], Vendor = vendors[0]},
                new Unit {Product = products[0], InventoryNumber = "3000-4", Location = locations[2], Vendor = vendors[0]},
                new Unit {Product = products[1], InventoryNumber = "5000-1", Location = locations[2], Vendor = vendors[1]},
                new Unit {Product = products[2], InventoryNumber = "PSM2-1", Location = locations[1], Vendor = vendors[1]},
                new Unit {Product = products[2], InventoryNumber = "PSM2-2", Location = locations[2], Vendor = vendors[1]}
            };
            context.Units.AddRange(units);
            context.SaveChanges();
            /*
            var tu1 = new List<Unit> {units[2], units[5]};
            var tu2 = new List<Unit> {units[3], units[4], units[6]};
            var transfers = new[]
            {
                new Transfer {OriginLocation = locations[0], DestinationLocation = locations[1], Units = tu1.AsQueryable()},
                new Transfer {OriginLocation = locations[0], DestinationLocation = locations[2], Units = tu2.AsQueryable()}
            };
            //context.Transfers.AddRange(transfers);
            context.SaveChanges();
            */
            var admin = new ApplicationUser { IsAdmin = true, }; // TODO: Add admin
            context.ApplicationUsers.Add(admin);
            context.SaveChanges();
        }
    }
}