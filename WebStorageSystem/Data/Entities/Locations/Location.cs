using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data.Entities.Locations
{
    public class Location : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location Type")]
        public LocationType LocationType { get; set; }

        public IEnumerable<Transfer> OriginTransfers { get; set; }
        public IEnumerable<Transfer> DestinationTransfers { get; set; }

        public Location()
        {
        }

        public Location(Location location, LocationType locationType) : base(location)
        {
            Name = location.Name;
            Description = location.Description;
            OriginTransfers = location.OriginTransfers;
            DestinationTransfers = location.DestinationTransfers;
            LocationType = locationType;
        }
    }
}
