using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationTypeModel : BaseEntityModelWithId
    {
        [Required, StringLength(100), Display(Name = "Name")]
        [JqueryDataTableColumn, SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [StringLength(500), Display(Name = "Description")]
        [JqueryDataTableColumn, SearchableString, Sortable]
        public string Description { get; set; }

        public List<LocationModel> Locations { get; set; }

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
