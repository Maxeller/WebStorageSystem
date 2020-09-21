using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Models.Product;
using WebStorageSystem.Models.Transfer;

namespace WebStorageSystem.Models.Transfer
{
    public class TransferUnit : BaseModel
    {
        public int TransferId { get; set; }
        public Transfer Transfer { get; set; }
        public int UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
