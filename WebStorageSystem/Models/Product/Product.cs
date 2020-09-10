using System.ComponentModel.DataAnnotations;

namespace WebStorageSystem.Models.Product
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public ProductType ProductType { get; set; }

        [Required]
        public Manufacturer Manufacturer { get; set; }

        [Required]
        public Vendor Vendor { get; set; } // TODO: move elsewhere?
    }
}
