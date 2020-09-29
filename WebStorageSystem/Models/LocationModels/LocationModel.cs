using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Models.LocationModels
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
