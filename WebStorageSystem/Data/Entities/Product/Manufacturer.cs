﻿using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Product
{
    public class Manufacturer : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IQueryable<Product> Products { get; set; }
    }
}
