using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;

namespace WebStorageSystem.Models
{
    public abstract class BaseEntityModel
    {
        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn, SearchableDateTime, Sortable]
        public abstract DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn, SearchableDateTime, Sortable]
        public abstract DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn, Sortable]
        public abstract bool IsDeleted { get; set; }

        [Timestamp]
        [JqueryDataTableColumn(Exclude = true)]
        public abstract byte[] RowVersion { get; set; }

        [JqueryDataTableColumn]
        public abstract Dictionary<string, string> Action { get; set; }
    }

    public abstract class BaseEntityModelWithId : BaseEntityModel
    {
        [JqueryDataTableColumn(Exclude = true)]
        public abstract int Id { get; set; }
    }
}
