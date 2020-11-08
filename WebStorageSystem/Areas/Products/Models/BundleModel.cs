﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class BundleModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        [JqueryDataTableColumn(Order = 40), SearchableString, Sortable(Default = true)]
        public string Name { get; set; }

        [Required, DisplayName("Serial Number")]
        [JqueryDataTableColumn(Order = 41), SearchableString, Sortable]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [DisplayName("Bundled Units")]
        [JqueryDataTableColumn(Exclude = true)]
        public IEnumerable<UnitModel> BundledUnits { get; set; }

        [Required, DisplayName("Bundled Units")]
        [JqueryDataTableColumn(Exclude = true)]
        public IEnumerable<int> BundledUnitsIds { get; set; }

        [DisplayName("# Units")]
        [JqueryDataTableColumn(Order = 42)]
        public int NumberOfUnits => BundledUnits.ToArray().Length;

        [Display(Name = "Creation Date")]
        [JqueryDataTableColumn(Order = 43)]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        [JqueryDataTableColumn(Order = 44)]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        [JqueryDataTableColumn(Order = 45)]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JqueryDataTableColumn(Order = 45)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
