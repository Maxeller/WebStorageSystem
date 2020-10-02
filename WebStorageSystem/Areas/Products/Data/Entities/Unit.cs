using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Areas.Products.Data.Entities
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

        public Bundle PartOfBundle { get; set; }

        public IEnumerable<TransferUnit> TransferredUnits { get; set; }
    }
}
