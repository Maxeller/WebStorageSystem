using WebStorageSystem.Areas.Products.Data.Entities;

namespace WebStorageSystem.Data.Entities.Transfers
{
    public class TransferUnit : BaseEntity
    {
        public int TransferId { get; set; }
        public Transfer Transfer { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
