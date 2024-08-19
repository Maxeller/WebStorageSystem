using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using WebStorageSystem.Areas.Defects.Models;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Models
{
    public class ApplicationUserModel : IdentityUser
    {
        public override string Id { get; set; }

        [Display(Name = "Username")]
        public override string UserName { get; set; }
        public override string NormalizedUserName { get; set; }
        public override string Email { get; set; }
        public override string NormalizedEmail { get; set; }
        public override bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }
        
        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }
        public override string PhoneNumber { get; set; }
        public override bool PhoneNumberConfirmed { get; set; }
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }

        [JsonIgnore]
        public IEnumerable<MainTransferModel> Transfers { get; set; }

        [JsonIgnore]
        public IEnumerable<DefectModel> ReportedDefects { get; set; }

        [JsonIgnore]
        public IEnumerable<DefectModel> CausedDefects { get; set; }

        [JsonIgnore]
        public IEnumerable<LocationModel> SubscribedLocations { get; set; }
    }
}
