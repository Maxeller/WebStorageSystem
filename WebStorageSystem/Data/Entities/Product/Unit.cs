using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebStorageSystem.Data.Entities.Transfer;

namespace WebStorageSystem.Data.Entities.Product
{
    public class Unit : BaseEntityWithId
    {
        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        public Product Product { get; set; }

        [Required]
        public Location.Location Location { get; set; }

        public Vendor Vendor { get; set; }

        public IQueryable<TransferUnit> TransferredUnits { get; set; }
    }
}
