﻿using System.ComponentModel;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class UnitBundleView
    {
        [DisplayName("Inventory Number")]
        public string InventoryNumber { get; set; }

        public Unit Unit { get; private set; }

        public int UnitId { get; set; }

        public Bundle Bundle { get; private set; }

        public int BundleId { get; set; }

        public bool IsBundle => Bundle != null;

    }
}