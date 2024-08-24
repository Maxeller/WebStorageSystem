using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebStorageSystem.Data.Entities.Transfers;
using System.Text.Json.Serialization;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Models.Transfers
{
    public class MainTransferModel : BaseEntityModelWithId
    {
        [Required]
        [Display(Name = "Transfer Number")]
        public string TransferNumber { get; set; }

        [Display(Name = "Transferred Units")]
        [JsonIgnore]
        public IEnumerable<SubTransferModel> SubTransfers { get; set; }

        [Display(Name = "Transfer State")]
        public TransferState State { get; set; }

        [Display(Name = "Time of Transfer")]
        public DateTime TransferTime { get; set; }

        public ApplicationUserModel User { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Destination Location")]
        public LocationModel DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public int DestinationLocationId { get; set; }

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
