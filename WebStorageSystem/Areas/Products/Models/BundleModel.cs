using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using IronBarCode;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Areas.Products.Models
{
    public class BundleModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)] public string Name { get; set; }

        [Required, DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; } //TODO: compatibility with code reader

        [DisplayName("Bundled Units")] public IEnumerable<UnitModel> BundledUnits { get; set; }

        [Required, DisplayName("Bundled Units")]
        public IEnumerable<int> BundledUnitsIds { get; set; }

        [DisplayName("Location")]
        public LocationModel Location { get; set; }

        [Required, DisplayName("Location")]
        public int LocationId { get; set; }

        [DisplayName("Default Location")]
        public LocationModel DefaultLocation { get; set; }

        [Required, DisplayName("Default Location")]
        public int DefaultLocationId { get; set; }

        [JsonIgnore]
        public IEnumerable<SubTransferModel> SubTransfers { get; set; }

        [DisplayName("# Units")]
        public int NumberOfUnits => BundledUnits?.ToArray().Length ?? 0;

        [JsonIgnore]
        public string BarCode
        {
            get
            {
                if (InventoryNumber == null) return "";
                GeneratedBarcode barcode = BarcodeWriter
                    .CreateBarcode(InventoryNumber, BarcodeEncoding.Code128)
                    .ResizeTo(250, 50)
                    .AddAnnotationTextAboveBarcode(Name)
                    .AddAnnotationTextBelowBarcode(InventoryNumber);
                return barcode.ToHtmlTag();
            }
        }

        [DisplayName("Has Defect")]
        public bool HasDefect { get; set; }

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

        public override bool Equals(object obj)
        {
            return InventoryNumber == ((BundleModel)obj)?.InventoryNumber;
        }
    }
}
