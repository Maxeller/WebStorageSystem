using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Defects.Models
{
    public class DefectModel : BaseEntityModelWithId
    {
        public override int Id { get; set; }

        [Required, StringLength(100), DisplayName("Defect Number")]
        public string DefectNumber { get; set; }

        public UnitModel Unit { get; set; }
        [Required, DisplayName("Unit")] 
        public int UnitId { get; set; }

        public ApplicationUserModel ReportedByUser { get; set; }

        [DisplayName("Reported by")]
        public string ReportedByUserId { get; set; }

        public ApplicationUserModel CausedByUser { get; set; }
        [DisplayName("Caused by")]
        public string CausedByUserId { get; set; }

        [Required]
        public string Description { get; set; }

        public string Notes { get; set; }

        public ImageEntityModel Image { get; set; }

        public DefectState State { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
    }
}
