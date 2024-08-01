using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Text.Json.Serialization;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitBundleModel : BaseEntityModelWithId
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; }

        public bool IsBundle { get; set; }

        public UnitModel Unit { get; set; }

        public BundleModel Bundle { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
