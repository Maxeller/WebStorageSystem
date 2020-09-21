using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Models.Location
{
    public class Location : BaseModel
    {
        public int Id { get; set; }

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
