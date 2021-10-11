using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Models
{
    public class TransferModel : BaseEntityModelWithId
    {
        [Required, Display(Name = "Transfer Number")]
        public string TransferNumber { get; set; }

        [Display(Name = "Origin Location")]
        public LocationModel OriginLocation { get; set; }

        [Required, Display(Name = "Origin Location")]
        public int OriginLocationId { get; set; }

        [Display(Name = "Destination Location")]
        public LocationModel DestinationLocation { get; set; }

        [Required, Display(Name = "Destination Location")]
        public int DestinationLocationId { get; set; }

        [Required, Display(Name = "Transfer State")]
        public TransferState State { get; set; }

        [Display(Name = "Time of Transfer")]
        public DateTime TransferTime { get; set; }

        [Display(Name = "Transferred Units")]
        public IEnumerable<UnitModel> Units { get; set; }

        [Required, Display(Name = "Transferred Units")]
        public IEnumerable<int> UnitsIds { get; set; }

        public ApplicationUserModel User { get; set; }

        public string UserId { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Modification Date")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}