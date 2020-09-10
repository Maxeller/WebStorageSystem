using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStorageSystem.Models.Product
{
    public class Unit
    {
        public int Id { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public string SerialNumber { get; set; } //TODO: compatibility with code reader

        [Required]
        public Location.Location Location { get; set; }
    }
}
