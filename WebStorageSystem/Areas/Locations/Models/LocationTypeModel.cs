using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Models.LocationModels
{
    public class LocationTypeModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public List<LocationModel> Locations { get; set; }
    }
}
