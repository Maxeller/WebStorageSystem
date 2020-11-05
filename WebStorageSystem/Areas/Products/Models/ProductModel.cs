using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ProductModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn(Order = 1), SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [Required, Display(Name = "Product Type")]
        [JqueryDataTableColumn(Exclude = true)]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product Type")]
        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public ProductTypeModel ProductType { get; set; }

        [Required, Display(Name = "Manufacturer")]
        [JqueryDataTableColumn(Exclude = true)]
        public int ManufacturerId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public ManufacturerModel Manufacturer { get; set; }

        public IEnumerable<UnitModel> Units { get; set; }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 40)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 41)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 42)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 43)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
