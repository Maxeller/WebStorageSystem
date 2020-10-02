using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Data.Entities
{
    public class Product : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public ProductType ProductType { get; set; }

        [Required]
        public Manufacturer Manufacturer { get; set; }

        public IEnumerable<Unit> Units { get; set; }
    }
}
