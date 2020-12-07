using System.Collections.Generic;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Identity;

namespace WebStorageSystem.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        [JqueryDataTableColumn(Order = 390), SearchableString, Sortable]
        public override string UserName { get; set; }

        [JqueryDataTableColumn(Order = 391), Searchable, Sortable]
        public bool IsAdmin { get; set; }

        public IEnumerable<TransferModel> Transfers { get; set; }
    }
}
