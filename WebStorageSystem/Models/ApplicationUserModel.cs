using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Attributes;
using Microsoft.AspNetCore.Identity;

namespace WebStorageSystem.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        [JqueryDataTableColumn(Exclude = true)]
        public override string Id { get; set; }

        [Display(Name = "Username")]
        [JqueryDataTableColumn(Order = 390), SearchableString, Sortable]
        public override string UserName { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string NormalizedUserName { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string Email { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string NormalizedEmail { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override bool EmailConfirmed { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string PasswordHash { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string SecurityStamp { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string ConcurrencyStamp { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override string PhoneNumber { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override bool PhoneNumberConfirmed { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override bool TwoFactorEnabled { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override bool LockoutEnabled { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public override int AccessFailedCount { get; set; }

        [JqueryDataTableColumn(Order = 391), Searchable, Sortable]
        public bool IsAdmin { get; set; }

        [JqueryDataTableColumn(Exclude = true)]
        public IEnumerable<TransferModel> Transfers { get; set; }
    }
}
