using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Models.ProductModels
{
    public class ProductTypeModel: BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        //public IEnumerable<Product> Products { get; set; }

    }
}
