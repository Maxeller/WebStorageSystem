using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationModel : BaseEntityModelWithId
    {
        [Required, StringLength(100), Display(Name = "Name")]
        [JqueryDataTableColumn, SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [StringLength(500)]
        [JqueryDataTableColumn, SearchableString, Sortable]
        public string Description { get; set; }

        [Required, Display(Name = "Location Type")]
        [JqueryDataTableColumn(Exclude = true)]
        public int LocationTypeId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public LocationTypeModel LocationType { get; set; }

        //public IQueryable<Transfer> OriginTransfers { get; set; }
        //public IQueryable<Transfer> DestinationTransfers { get; set; }

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
