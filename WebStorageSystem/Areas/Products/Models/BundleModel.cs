using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class BundleModel : BaseEntityModelWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [DisplayName("Bundled Units")]
        public IEnumerable<UnitModel> BundledUnits { get; set; }

        [Required]
        [DisplayName("Bundled Units")]
        public IEnumerable<int> BundledUnitsIds { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
