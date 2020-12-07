﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Models
{
    public class TransferModel : BaseEntityModelWithId
    {
        [Required, Display(Name = "Transfer Number")]
        [JqueryDataTableColumn(Order = 1), SearchableString, Sortable(Default = true)]
        public string TransferNumber { get; set; }

        [Display(Name = "Origin Location")]
        [JqueryDataTableColumn, NestedSortable, NestedSearchable]
        public LocationModel OriginLocation { get; set; }

        [Required, Display(Name = "Origin Location")]
        [JqueryDataTableColumn(Exclude = true)]
        public int OriginLocationId { get; set; }

        [Display(Name = "Destination Location")]
        [JqueryDataTableColumn, NestedSortable, NestedSearchable]
        public LocationModel DestinationLocation { get; set; }

        [Required, Display(Name = "Destination Location")]
        [JqueryDataTableColumn(Exclude = true)]
        public int DestinationLocationId { get; set; }

        [Display(Name = "Time of Transfer")]
        [JqueryDataTableColumn(Order = 361), SearchableDateTime, Sortable]
        public DateTime TransferTime { get; set; }

        [Display(Name = "Transferred Units")]
        [JqueryDataTableColumn(Order = 360)]
        public IEnumerable<UnitModel> TransferredUnits { get; set; }

        [Required, Display(Name = "Transferred Units")]
        [JqueryDataTableColumn(Exclude = true)]
        public IEnumerable<int> TransferredUnitsIds { get; set; }

        public ApplicationUser User { get; set; }

        [JqueryDataTableColumn(Order = 395), SearchableString(EntityProperty = "User.UserName."), Sortable(EntityProperty = "User.UserName")]
        public string Username => User != null ? User.UserName : "";

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 396)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Modification Date")]
        [JqueryDataTableColumn(Order = 397)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 398)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 399)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}