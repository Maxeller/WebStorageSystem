using NetBarcode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebStorageSystem.Areas.Defects.Models;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Transfers;
using Type = NetBarcode.Type;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitModel : BaseEntityModelWithId
    {
        [Required, DisplayName("Inventory Number"), StringLength(100)]
        public string InventoryNumber { get; set; } //TODO: compatibility with code reader

        [DisplayName("Serial Number"), StringLength(100)]
        public string SerialNumber { get; set; }

        public ProductModel Product { get; set; }

        [Required, DisplayName("Product")]
        public int ProductId { get; set; }

        public LocationModel Location { get; set; }

        [Required, DisplayName("Location")]
        public int LocationId { get; set; }

        public LocationModel DefaultLocation { get; set; }

        [Required, DisplayName("Default Location")]
        public int DefaultLocationId { get; set; }

        public VendorModel Vendor { get; set; }

        [DisplayName("Vendor")]
        public int? VendorId { get; set; }

        [DisplayName("Part of Bundle")]
        public BundleModel PartOfBundle { get; set; }

        [DisplayName("Part of Bundle")]
        public int? PartOfBundleId { get; set; }

        [DisplayName("Shelf Number"), StringLength(100)]
        public string ShelfNumber { get; set; }

        public string Notes { get; set; }

        [JsonIgnore]
        public string BarCode
        {
            get
            {
                if (InventoryNumber == null) return "";
                var barcode = new Barcode(InventoryNumber, Type.Code128);
                return barcode.GetBase64Image();
            }
        }

        [DisplayName("Last Transfer Date")]
        public DateTime? LastTransferTime { get; set; }

        [DisplayName("Last Check Time")]
        public DateTime? LastCheckTime { get; set; }

        [DisplayName("Has Defect")]
        public bool HasDefect { get; set; }

        [JsonIgnore]
        public IEnumerable<SubTransferModel> Transfers { get; set; }

        [JsonIgnore]
        public IEnumerable<DefectModel> Defects { get; set; }

        public string InventoryNumberProduct
        {
            get
            {
                if (Product == null) return InventoryNumber;
                return InventoryNumber + " (" + Product.Name + ")";
            }
        }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }

        [JsonIgnore]
        public override byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
