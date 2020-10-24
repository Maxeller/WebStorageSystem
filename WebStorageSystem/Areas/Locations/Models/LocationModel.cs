using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationModel : BaseEntityModelWithId
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

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
