using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Data.Entities.Products
{
    public class Manufacturer : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
