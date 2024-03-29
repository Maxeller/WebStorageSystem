﻿using System;
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

        [Required]
        public Unit Unit { get; set; }
        public int UnitId { get; set; }

        [Required]
        public ApplicationUser ReportedByUser { get; set; }
        public string ReportedByUserId { get; set; }

        public ApplicationUser CausedByUser { get; set; }
        public string CausedByUserId { get; set; }

        [Required]
        public string Description { get; set; }

        public string Notes { get; set; }

        public ImageEntity Image { get; set; }

        public DefectState State { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
    }

    public enum DefectState
    {
        Broken,
        InRepair,
        Repaired
    }
}
