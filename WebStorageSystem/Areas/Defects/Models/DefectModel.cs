﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Defects.Models
{
    public class DefectModel : BaseEntityModelWithId
    {
        [Required, StringLength(100), DisplayName("Defect Number")]
        public string DefectNumber { get; set; }

        public UnitModel Unit { get; set; }
        [Required, DisplayName("Unit")] 
        public int UnitId { get; set; }

        public ApplicationUserModel CreatedByUser { get; set; }

        [DisplayName("Created by")]
        public string CreatedByUserId { get; set; }

        public ApplicationUserModel DiscoveredByUser { get; set; }
        [DisplayName("Discovered by")]
        public string DiscoveredByUserId { get; set; }

        [Required]
        public string Description { get; set; }

        public string Notes { get; set; }

        public ImageEntityModel Image { get; set; }
        public int? ImageId { get; set; }

        public DefectState State { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }

        public override byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public override Dictionary<string, string> Action { get; set; }

        public override int Id { get; set; }
    }
}
