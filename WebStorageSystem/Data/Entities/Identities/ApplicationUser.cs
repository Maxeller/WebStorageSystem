using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Data.Entities.Transfers;

namespace WebStorageSystem.Data.Entities.Identities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }

        public IEnumerable<Transfer> Transfers { get; set; }
        public IEnumerable<Defect> ReportedDefects { get; set; }
        public IEnumerable<Defect> CausedDefects { get; set; }
    }
}
