using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Data.Entities.Products
{
    public class Bundle : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        public IEnumerable<Unit> BundledUnits { get; set; }
    }
}
