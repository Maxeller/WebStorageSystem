using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Data.Entities.Products
{
    public class ProductType : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
