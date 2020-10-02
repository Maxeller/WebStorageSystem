using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class Manufacturer : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
