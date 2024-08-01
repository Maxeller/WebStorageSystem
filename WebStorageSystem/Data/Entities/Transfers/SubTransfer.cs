using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Entities;

namespace WebStorageSystem.Data.Entities.Transfers
{
    public class SubTransfer : BaseEntityWithId
    {
        [Required]
        public MainTransfer MainTransfer { get; set; }

        [Required]
        public int MainTransferId { get; set; }

        [Required]
        [Display(Name = "Origin Location")]
        public Location OriginLocation { get; set; }

        [Required]
        [Display(Name = "Origin Location")]
        public int OriginLocationId { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public Location DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public int DestinationLocationId { get; set; }

        [Display(Name = "Transferred Unit")]
        public Unit Unit { get; set; }
        [Display(Name = "Transferred Unit")]
        public int UnitId { get; set; }

        [Display(Name = "Transferred Bundle")]
        public Bundle Bundle { get; set; }
        [Display(Name = "Transferred Bundle")]
        public int BundleId { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
