using System.ComponentModel;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitBundleViewModel
    {
        [DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; }

        public UnitModel Unit { get; set; }
        public int? UnitId { get; set; }

        public BundleModel Bundle { get; set; }
        public int? BundleId { get; set; }

        public LocationModel Location { get; set; }
        public int LocationId { get; set; }

        public LocationModel DefaultLocation { get; set; }
        public int DefaultLocationId { get; set; }

        public bool HasDefect { get; set; }

        public string TableName { get; set; }

        public bool IsBundle => BundleId != null;
    }
}
