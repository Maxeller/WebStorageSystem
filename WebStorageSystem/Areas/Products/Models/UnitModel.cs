using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitModel : BaseEntityWithId
    {
        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        public ProductModel Product { get; set; }

        [Required]
        [DisplayName("Product")]
        public int ProductId { get; set; }

        public LocationModel Location { get; set; }

        [Required]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        public VendorModel Vendor { get; set; }

        [DisplayName("Vendor")]
        public int? VendorId { get; set; }
        /*
        public BundleModel PartOfBundle { get; set; }
        
        [DisplayName("Part of Bundle")]
        public int PartOfBundleId { get; set; }

        //public IEnumerable<TransferUnitModel> TransferredUnits { get; set; }
        */
    }
}
