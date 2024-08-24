using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Transfers;

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

        [JsonIgnore, XmlIgnore]
        public IEnumerable<SubTransferModel> OriginTransfers { get; set; }

        [JsonIgnore, XmlIgnore]
        public IEnumerable<SubTransferModel> DestinationTransfers { get; set; }

        [JsonIgnore, XmlIgnore]
        public IEnumerable<MainTransferModel> DestinationMainTransfers { get; set; }

        [JsonIgnore, XmlIgnore]
        public IEnumerable<UnitModel> Units { get; set; }

        [JsonIgnore, XmlIgnore]
        public IEnumerable<UnitModel> DefaultUnits { get; set; }

        [JsonIgnore, XmlIgnore]
        public IEnumerable<ApplicationUserModel> UsersSubscribed { get; set; }

        [Display(Name = "Creation Date")]
        public override DateTime CreatedDate { get; set; }

        [Display(Name = "Last Modification")]
        public override DateTime ModifiedDate { get; set; }

        [Display(Name = "Deleted")]
        public override bool IsDeleted { get; set; }

        public override byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull), XmlIgnore]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
