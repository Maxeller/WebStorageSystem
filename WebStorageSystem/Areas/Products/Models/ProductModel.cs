using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebStorageSystem.Models;

namespace WebStorageSystem.Areas.Products.Models
{
    public class ProductModel : BaseEntityModelWithId
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

        public IEnumerable<UnitModel> Units { get; set; }

        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override Dictionary<string, string> Action { get; set; }
        public override int Id { get; set; }
    }
}
