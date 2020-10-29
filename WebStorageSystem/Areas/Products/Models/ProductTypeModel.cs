using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ProductTypeModel: BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn, SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [StringLength(500)]
        [JqueryDataTableColumn, SearchableString, Sortable]
        public string Description { get; set; }

        public IEnumerable<ProductModel> Products { get; set; }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn]
        public override bool IsDeleted { get; set; }

        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn]
        public override Dictionary<string, string> Action { get; set; }

        public override int Id { get; set; }
    }
}
