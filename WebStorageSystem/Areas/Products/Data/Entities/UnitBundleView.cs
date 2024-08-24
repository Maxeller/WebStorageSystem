using System.ComponentModel;
using WebStorageSystem.Areas.Locations.Data.Entities;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class UnitBundleView
    {
        [DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; }

        public Unit Unit { get; set; }
        public int? UnitId { get; set; }

        public Bundle Bundle { get; set; }
        public int? BundleId { get; set; }

        public Location Location { get; set; }
        public int LocationId { get; set; }

        public Location DefaultLocation { get; set; }
        public int DefaultLocationId { get; set; }

        public bool HasDefect { get; set; }

        public string TableName { get; set; }

        public bool IsBundle => BundleId != null;

    }
}
