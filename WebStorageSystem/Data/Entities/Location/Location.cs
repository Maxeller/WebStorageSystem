using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Location
{
    public class Location : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Location Type")]
        public LocationType LocationType { get; set; }

        public IQueryable<Transfer.Transfer> OriginTransfers { get; set; }
        public IQueryable<Transfer.Transfer> DestinationTransfers { get; set; }
    }
}
