using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Models.Transfer;

namespace WebStorageSystem.Models.Product
{
    public class Unit : BaseModelWithId
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
