﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Product
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

        public IQueryable<Unit> Units { get; set; }
    }
}