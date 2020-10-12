using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Models
{
    public class BundleModel : BaseEntityWithId
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
    }
}
