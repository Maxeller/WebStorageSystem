using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitModel : BaseEntityModelWithId
    {
        [Required, DisplayName("Serial Number")]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        public ProductModel Product { get; set; }

        [Required, DisplayName("Product")]
        public int ProductId { get; set; }

        public LocationModel Location { get; set; }

        [Required, DisplayName("Location")]
        public int LocationId { get; set; }

        [Required, DisplayName("Default Location")]
        public int DefaultLocationId { get; set; }

        public LocationModel DefaultLocation { get; set; }

        public VendorModel Vendor { get; set; }

        [DisplayName("Vendor")]
        public int? VendorId { get; set; }

        [DisplayName("Part of Bundle")]
        public BundleModel PartOfBundle { get; set; }

        [DisplayName("Part of Bundle")]
        public int? PartOfBundleId { get; set; }

        public IEnumerable<TransferModel> Transfers { get; set; }

        [DisplayName("Shelf Number")]
        public string ShelfNumber { get; set; }

        public string Notes { get; set; }

        [DisplayName("Last Transfer Date")]
        public DateTime? LastTransferTime { get; set; }

        [DisplayName("Last Check Time")]
        public DateTime? LastCheckTime { get; set; }

        public string SerialNumberProduct
        {
            get
            {
                if (Product == null) return SerialNumber;
                return SerialNumber + " (" + Product.Name + ")";
            }
        }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
