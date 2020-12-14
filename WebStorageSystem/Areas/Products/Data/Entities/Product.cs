using System;
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
        [StringLength(100)]
        public string ProductNumber { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        
        [StringLength(200)]
        public string Webpage { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public ProductType ProductType { get; set; }
        public int ProductTypeId { get; set; }

        [Required]
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }

        public IEnumerable<Unit> Units { get; set; }
        public override DateTime CreatedDate { get; set; }
        public override DateTime ModifiedDate { get; set; }
        public override bool IsDeleted { get; set; }
        public override byte[] RowVersion { get; set; }
        public override int Id { get; set; }
    }
}
