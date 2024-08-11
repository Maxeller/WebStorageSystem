using System.ComponentModel;

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

        public string TableName { get; set; }

        public bool IsBundle => BundleId != null;

    }
}
