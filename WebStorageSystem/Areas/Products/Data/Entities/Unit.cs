using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class Unit : BaseEntityWithId
    {
        [Required, DisplayName("Inventory Number"), StringLength(100)]
        public string InventoryNumber { get; set; } //TODO: compatibility with code reader

        [DisplayName("Serial Number"), StringLength(100)]
        public string SerialNumber { get; set; }

        
        public Product Product { get; set; }
        
        [Required]
        public int ProductId { get; set; }

        
        public Location Location { get; set; }
        
        [Required]
        public int LocationId { get; set; }

        
        public Location DefaultLocation { get; set; }
        
        [Required]
        public int DefaultLocationId { get; set; }

        public Vendor Vendor { get; set; }
        public int? VendorId { get; set; }

        public Bundle PartOfBundle { get; set; }
        public int? PartOfBundleId { get; set; }

        [DisplayName("Shelf Number"), StringLength(100)]
        public string ShelfNumber { get; set; }

        public string Notes { get; set; }

        public DateTime? LastTransferTime { get; set; } // TODO: Set while Transfering

        public DateTime? LastCheckTime { get; set; }

        public IEnumerable<SubTransfer> SubTransfers { get; set; }
        public IEnumerable<Defect> Defects { get; set; }
        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
