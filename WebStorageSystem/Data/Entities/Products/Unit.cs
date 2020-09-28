using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Data.Entities.Locations;

namespace WebStorageSystem.Data.Entities.Products
{
    public class Unit : BaseEntityWithId
    {
        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        public Product Product { get; set; }

        [Required]
        public Location Location { get; set; }

        public Vendor Vendor { get; set; }

        public IEnumerable<TransferUnit> TransferredUnits { get; set; }
    }
}
