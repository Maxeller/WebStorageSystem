using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Models.ProductModels
{
    public class ManufacturerModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        //public IEnumerable<Product> Products { get; set; }
    }
}
