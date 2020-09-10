using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Models.Location;
using WebStorageSystem.Models.Product;

namespace WebStorageSystem.Models.Transfers
{
    public class Transfer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Origin Location")]
        public Location.Location OriginLocation { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public Location.Location DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Transferred Units")]
        public IQueryable<Unit> TransferredUnits { get; set; }

        //TODO: User
    }
}
