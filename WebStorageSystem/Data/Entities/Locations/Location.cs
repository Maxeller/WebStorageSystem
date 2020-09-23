using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        public IQueryable<Transfer> OriginTransfers { get; set; }
        public IQueryable<Transfer> DestinationTransfers { get; set; }
    }
}
