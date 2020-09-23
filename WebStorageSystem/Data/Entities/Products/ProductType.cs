using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Products
{
    public class ProductType : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public IQueryable<Product> Products { get; set; }
    }
}
