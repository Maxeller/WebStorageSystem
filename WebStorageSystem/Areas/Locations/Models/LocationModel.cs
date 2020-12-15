using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn(Order = 100), SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [StringLength(500)]
        [JqueryDataTableColumn(Order = 102), SearchableString, Sortable]
        public string Description { get; set; }

        [StringLength(200)]
        [JqueryDataTableColumn(Order = 103), SearchableString, Sortable]
        public string Address { get; set; }

        [Required, Display(Name = "Location Type")]
        [JqueryDataTableColumn(Exclude = true)]
        public int LocationTypeId { get; set; }

        [JqueryDataTableColumn, NestedSearchable, NestedSortable]
        public LocationTypeModel LocationType { get; set; }

        public IEnumerable<TransferModel> OriginTransfers { get; set; }

        public IEnumerable<TransferModel> DestinationTransfers { get; set; }

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 146)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 147)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 148)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 149)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
