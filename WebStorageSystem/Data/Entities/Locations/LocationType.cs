using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Locations
{
    public class LocationType : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IQueryable<Location> Locations { get; set; }
    }
}
