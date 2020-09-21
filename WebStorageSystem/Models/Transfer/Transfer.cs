using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WebStorageSystem.Models.Identity;
using WebStorageSystem.Models.Product;

namespace WebStorageSystem.Models.Transfer
{
    public class Transfer : BaseModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Origin Location")]
        public Location.Location OriginLocation { get; set; }

        [Required]
        [Display(Name = "Destination Location")]
        public Location.Location DestinationLocation { get; set; }

        [Required]
        [Display(Name = "Time of Transfer")]
        //TODO: DateTime 
        public DateTime TransferTime { get; set; }

        [Required]
        [Display(Name = "Transferred Units")]
        public IQueryable<TransferUnit> TransferredUnits { get; set; }

        public ApplicationUser User { get; set; }
    }
}
