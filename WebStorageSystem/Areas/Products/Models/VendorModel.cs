using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class VendorModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn(Order = 150), SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        [JqueryDataTableColumn(Order = 151), SearchableString, Sortable]
        public string Address { get; set; }

        [Required, Phone, StringLength(50)]
        [JqueryDataTableColumn(Order = 152), SearchableString, Sortable]
        public string Phone { get; set; }

        [Required, EmailAddress, StringLength(200)]
        [JqueryDataTableColumn(Order = 153), SearchableString, Sortable]
        public string Email { get; set; }

        [Required, StringLength(200)]
        [JqueryDataTableColumn(Order = 154), SearchableString, Sortable]
        public string Website { get; set; }

        [JsonIgnore]
        public IEnumerable<UnitModel> Units { get; set; }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 176)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 177)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 178)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 179)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
