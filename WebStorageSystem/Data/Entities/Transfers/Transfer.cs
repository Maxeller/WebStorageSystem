using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;

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
        public DateTime TransferTime { get; set; }

        [Required]
        [Display(Name = "Transferred Units")]
        public IEnumerable<TransferUnit> TransferredUnits { get; set; }

        public ApplicationUser User { get; set; }
        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
