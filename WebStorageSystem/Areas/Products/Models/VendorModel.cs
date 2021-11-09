using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class VendorModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, StringLength(200)]
        public string Address { get; set; }

        [Required, Phone, StringLength(50)]
        public string Phone { get; set; }

        [Required, EmailAddress, StringLength(200)]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string Website { get; set; }

        [JsonIgnore]
        public IEnumerable<UnitModel> Units { get; set; }

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
