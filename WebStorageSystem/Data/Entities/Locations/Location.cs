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

        [Required]
        [Display(Name = "Location Type")]
        public LocationType LocationType { get; set; }

        public IEnumerable<Transfer> OriginTransfers { get; set; }
        public IEnumerable<Transfer> DestinationTransfers { get; set; }
    }
}
