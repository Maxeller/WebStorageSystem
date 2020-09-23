using System.Linq;
using Microsoft.AspNetCore.Identity;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data.Entities.Identities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }

        public IQueryable<Transfer> Transfers { get; set; }
    }
}
