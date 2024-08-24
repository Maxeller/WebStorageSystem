using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Areas.Identity.Pages.Account.Manage
{
    public partial class SubscribeLocationsModel : PageModel
    {
        private readonly AppUserManager _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly LocationService _locationService;


        public SubscribeLocationsModel(AppUserManager userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper, LocationService locationService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _locationService = locationService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Subscribed Locations")]
            public List<int> SubscribedLocationsId { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var locations = await _locationService.GetLocationsAsync();
            var subscribedLocations = await _userManager.GetSubscribedLocations(user);

            ViewData["Locations"] = new MultiSelectList(locations, "Id", "Name", (subscribedLocations.ToArray() ?? Array.Empty<Location>()).Select(x => x.Id).ToList());
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var result = await _userManager.SaveSubscribedLocationsAsync(user, Input.SubscribedLocationsId);
            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set locations.";
                return RedirectToPage();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
