using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Data.Entities.Transfers
{
    public class MainTransfer : BaseEntityWithId
    {
        [Required]
        [Display(Name = "Transfer Number")]
        public string TransferNumber { get; set; }

        [Required]
        [Display(Name = "Transferred Units")]
        public IEnumerable<SubTransfer> SubTransfers { get; set; }

        [Required]
        [Display(Name = "Transfer State")]
        public TransferState State { get; set; }

        [Required]
        [Display(Name = "Time of Transfer")]
        public DateTime TransferTime { get; set; }

        public ApplicationUser User { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        public Location DestinationLocation { get; set; }

        [Required]
        public int DestinationLocationId { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }

    public enum TransferState : ushort
    {
        Prepared = 1,
        Transferred = 2
    }
}
