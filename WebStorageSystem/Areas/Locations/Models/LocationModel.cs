using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Location Type")]
        public int LocationTypeId { get; set; }

        [Display(Name = "Location Type")]
        public LocationTypeModel LocationType { get; set; }

        //public IQueryable<Transfer> OriginTransfers { get; set; }
        //public IQueryable<Transfer> DestinationTransfers { get; set; }
    }
}
