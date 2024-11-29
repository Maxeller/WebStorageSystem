using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data.Entities.Identities
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<MainTransfer> Transfers { get; set; }
        public IEnumerable<Defect> CreatedDefects { get; set; }
        public IEnumerable<Defect> DiscoveredDefects { get; set; }
        public ICollection<Location> SubscribedLocations { get; set; }
    }
}
