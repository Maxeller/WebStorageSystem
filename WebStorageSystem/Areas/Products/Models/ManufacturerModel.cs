using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ManufacturerModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn(Order = 70), SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [StringLength(500)]
        [JqueryDataTableColumn(Order = 71), SearchableString, Sortable]
        public string Description { get; set; }

        [JsonIgnore]
        public IEnumerable<ProductModel> Products { get; set; }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 92)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 93)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 94)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 95)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
