using WebStorageSystem.Data.Entities.Product;

namespace WebStorageSystem.Data.Entities.Transfer
{
    public class TransferUnit : BaseEntity
    {
        public int TransferId { get; set; }
        public Transfer Transfer { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
