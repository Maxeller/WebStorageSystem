﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Areas.Locations.Data.Entities
{
    public class Location : BaseEntityWithId
    {
        public override int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Location Type")]
        public LocationType LocationType { get; set; }
        public int LocationTypeId { get; set; }

        public IEnumerable<Transfer> OriginTransfers { get; set; }

        public IEnumerable<Transfer> DestinationTransfers { get; set; }

        public IEnumerable<Unit> Units { get; set; }

        public IEnumerable<Unit> DefaultUnits { get; set; }

        public override DateTime CreatedDate { get; set; }

        public override DateTime ModifiedDate { get; set; }

        public override bool IsDeleted { get; set; }

        public override byte[] RowVersion { get; set; }
    }
}
