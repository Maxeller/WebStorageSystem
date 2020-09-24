using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Models.LocationModels
{
    public class LocationTypeModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        //public IQueryable<LocationModel> Locations { get; set; }
    }
}
