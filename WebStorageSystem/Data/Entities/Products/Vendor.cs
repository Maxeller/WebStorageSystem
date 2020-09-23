using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebStorageSystem.Data.Entities.Products
{
    public class Vendor : BaseEntityWithId
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [Phone]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        public IQueryable<Unit> Units { get; set; }
    }
}
