using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Locations;

namespace WebStorageSystem.Data.Entities.Transfers
{
    public class Transfer : BaseEntityWithId
    {
        [Required]
        [Display(Name = "Origin Location")]
        public Location OriginLocation { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public Location DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Time of Transfer")]
        //TODO: DateTime 
        public DateTime TransferTime { get; set; }

        [Required]
        [Display(Name = "Transferred Units")]
        public IEnumerable<TransferUnit> TransferredUnits { get; set; }

        public ApplicationUser User { get; set; }
    }
}
