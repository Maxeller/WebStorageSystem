using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ManufacturerModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IEnumerable<ProductModel> Products { get; set; }
    }
}
