using System;
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
        public int ProductId { get; set; }

        [Required]
        public Location Location { get; set; }
        public int LocationId { get; set; }

        [Required]
        public Location DefaultLocation { get; set; }
        public int DefaultLocationId { get; set; }

        public Vendor Vendor { get; set; }
        public int? VendorId { get; set; }

        public Bundle PartOfBundle { get; set; }
        public int? PartOfBundleId { get; set; }

        public string ShelfNumber { get; set; }

        public string Notes { get; set; }

        public DateTime? LastTransferTime { get; set; } // TODO: Set while Transfering

        public DateTime? LastCheckTime { get; set; }

        public IEnumerable<Transfer> Transfers { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
