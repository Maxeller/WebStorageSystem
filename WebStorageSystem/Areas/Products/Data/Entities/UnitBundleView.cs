using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class UnitBundleView
    {
        public UnitBundleView(int id, string inventoryNumber, Unit unit, Bundle bundle)
        {
            Id = id;
            InventoryNumber = inventoryNumber;
            Unit = unit;
            UnitId = unit.Id;
            Bundle = bundle;
            BundleId = bundle.Id;
            IsBundle = Bundle == null;
        }

        public int Id { get; set; }

        [Required, DisplayName("Inventory Number")]
        public string InventoryNumber { get; private set; }

        public bool IsBundle { get; private set; }

        public Unit Unit { get; private set; }

        public int UnitId { get; set; }

        public Bundle Bundle { get; private set; }

        public int BundleId { get; set; }

    }
}
