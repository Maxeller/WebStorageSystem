using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Data.Entities.Identities;
using Microsoft.EntityFrameworkCore;

namespace WebStorageSystem.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Roles = "Admin")]
    public partial class AssignRolesModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AssignRolesModel(AppUserManager userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "User")]
            public string UserId { get; set; }

            [Display(Name = "Roles")]
            public List<string> RolesId { get; set; }
        }

        public class UserModel
        {
            public UserModel(string id, string namePlusRole)
            {
                Id = id;
                NamePlusRole = namePlusRole;
            }

            public string Id { get; set; }
            public string NamePlusRole { get; set; }
        }

        private async Task LoadAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersModel = new List<UserModel>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                string namePlusRole = user.UserName + " (" + string.Join(",", userRoles) + ")";
                var model = new UserModel(user.Id, namePlusRole);
                usersModel.Add(model);
            }
            var roles = await _roleManager.Roles.ToListAsync();

            ViewData["Users"] = new SelectList(usersModel, "Id", "NamePlusRole");
            ViewData["Roles"] = new MultiSelectList(roles, "Id", "Name");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == Input.UserId);
            if (user == null)
            {
                StatusMessage = "User was not found";
                return RedirectToPage();
            }

            foreach (string roleId in Input.RolesId)
            {
                var role = await _roleManager.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
                if (role == null)
                {
                    StatusMessage = "Role was not found";
                    return RedirectToPage();
                }
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            await _signInManager.RefreshSignInAsync(currentUser);
            StatusMessage = "Roles has been updated";
            return RedirectToPage();
        }
    }
}
