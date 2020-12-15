using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitModel : BaseEntityModelWithId
    {
        [Required, DisplayName("Serial Number")]
        [JqueryDataTableColumn(Order = 1), SearchableString, Sortable(Default = true)]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required, DisplayName("Product")]
        [JqueryDataTableColumn(Exclude = true)]
        public int ProductId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public ProductModel Product { get; set; }

        [Required, DisplayName("Location")]
        [JqueryDataTableColumn(Exclude = true)]
        public int LocationId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public LocationModel Location { get; set; }

        [DisplayName("Vendor")]
        [JqueryDataTableColumn(Exclude = true)]
        public int? VendorId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public VendorModel Vendor { get; set; }

        [DisplayName("Part of Bundle")]
        [JqueryDataTableColumn(Exclude = true)]
        public int? PartOfBundleId { get; set; }

        [DisplayName("Part of Bundle")]
        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public BundleModel PartOfBundle { get; set; }

        public IEnumerable<TransferModel> Transfers { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public string SerialNumberProduct
        {
            get
            {
                if (Product == null) return SerialNumber;
                return SerialNumber + " (" + Product.Name + ")";
            }
        }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 206)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 207)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 208)]
        public override bool IsDeleted { get; set; }

        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 209)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
