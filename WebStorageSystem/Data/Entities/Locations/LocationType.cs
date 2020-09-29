﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Data.Entities.Locations
{
    public class LocationType : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IEnumerable<Location> Locations { get; set; }

        public LocationType()
        {
        }

        public LocationType(LocationType locationType, IEnumerable<Location> locations) : base(locationType)
        {
            Name = locationType.Name;
            Description = locationType.Description;
            Locations = locations;
        }
    }
}
