using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Locations.Models
{
    public class LocationModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [Required, Display(Name = "Location Type")]
        public int LocationTypeId { get; set; }

        public LocationTypeModel LocationType { get; set; }

        [JsonIgnore]
        public IEnumerable<TransferModel> OriginTransfers { get; set; }

        [JsonIgnore]
        public IEnumerable<TransferModel> DestinationTransfers { get; set; }

        [JsonIgnore]
        public IEnumerable<UnitModel> Units { get; set; }

        [JsonIgnore]
        public IEnumerable<UnitModel> DefaultUnits { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
