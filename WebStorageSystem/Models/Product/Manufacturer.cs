using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Models.Product
{
    public class Manufacturer : BaseModelWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IQueryable<Product> Products { get; set; }
    }
}
