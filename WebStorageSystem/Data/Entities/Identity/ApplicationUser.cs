using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace WebStorageSystem.Data.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }

        public IQueryable<Transfer.Transfer> Transfers { get; set; }
    }
}
