using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Models;
using System.Text.Json.Serialization;

namespace WebStorageSystem.Models.Transfers
{
    public class SubTransferModel : BaseEntityModelWithId
    {
        [Required]
        public MainTransferModel MainTransfer { get; set; }

        [Required]
        public int MainTransferId { get; set; }

        [Required]
        [Display(Name = "Origin Location")]
        public LocationModel OriginLocation { get; set; }
        public int OriginLocationId { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public LocationModel DestinationLocation { get; set; }
        public int DestinationLocationId { get; set; }

        [Required]
        public UnitModel Unit { get; set; }
        public int? UnitId { get; set; }

        [Display(Name = "Transferred Bundle")]
        public BundleModel Bundle { get; set; }
        public int? BundleId { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }

        [JsonIgnore]
        public override byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
