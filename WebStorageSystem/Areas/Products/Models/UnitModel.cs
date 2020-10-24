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
        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        [DisplayName("Product")]
        public int ProductId { get; set; }
        public ProductModel Product { get; set; }
        
        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }
        public LocationModel Location { get; set; }
        
        [DisplayName("Vendor")]
        public int? VendorId { get; set; }
        public VendorModel Vendor { get; set; }

        

        [DisplayName("Part of Bundle")]
        public int? PartOfBundleId { get; set; }
        public BundleModel PartOfBundle { get; set; }

        //public IEnumerable<TransferUnitModel> TransferredUnits { get; set; }

        public string SerialNumberProduct
        {
            get
            {
                if (Product == null) return SerialNumber;
                return SerialNumber + " (" + Product.Name + ")";
            }
        }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
