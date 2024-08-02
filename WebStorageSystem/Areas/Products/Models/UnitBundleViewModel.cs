using System.ComponentModel;

namespace WebStorageSystem.Areas.Products.Models
{
    public class UnitBundleViewModel
    {
        [DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; }

        public UnitModel Unit { get; set; }
        public int UnitId { get; set; }

        public BundleModel Bundle { get; set; }
        public int BundleId { get; set; }

        public bool IsBundle => Bundle != null;
    }
}
