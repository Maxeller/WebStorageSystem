using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Data.Entities;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ProductModel : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Product Type")]
        public ProductTypeModel ProductType { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        public ManufacturerModel Manufacturer { get; set; }

        //public IEnumerable<UnitModel> Units { get; set; }
    }
}
