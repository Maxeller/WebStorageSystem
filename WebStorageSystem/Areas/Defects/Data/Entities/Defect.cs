using System;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Areas.Defects.Data.Entities
{
    public class Defect : BaseEntityWithId
    {
        public override int Id { get; set; }

        [Required, StringLength(100)]
        public string DefectNumber { get; set; }

        
        public Unit Unit { get; set; }
        [Required]
        public int UnitId { get; set; }

        public ApplicationUser ReportedByUser { get; set; }
        [Required]
        public string ReportedByUserId { get; set; }

        public ApplicationUser CausedByUser { get; set; }
        public string CausedByUserId { get; set; }
            
        [Required, StringLength(500)]
        public string Description { get; set; }

        [StringLength(2000)]
        public string Notes { get; set; }

        public ImageEntity Image { get; set; }
        public int? ImageId { get; set; }

        public DefectState State { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
    }

    public enum DefectState : ushort
    {
        [Display(Name = "Broken")]
        Broken = 1,
        [Display(Name = "In repair")]
        InRepair = 2,
        [Display(Name = "Repaired")]
        Repaired = 3
    }
}
