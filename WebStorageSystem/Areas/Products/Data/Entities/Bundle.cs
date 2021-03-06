﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class Bundle : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        public IEnumerable<Unit> BundledUnits { get; set; }

        // TODO: Transfers?
        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
