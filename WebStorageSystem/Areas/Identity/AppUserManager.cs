using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem.Areas.Identity
{
    public class AppUserManager : UserManager<ApplicationUser>
    {
        private readonly AppDbContext _context;

        public AppUserManager(IUserStore<ApplicationUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<ApplicationUser> passwordHasher, IEnumerable<IUserValidator<ApplicationUser>> userValidators, IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger, AppDbContext context) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _context = context;
        }

        public async Task<List<Location>> GetSubscribedLocations(ApplicationUser user)
        {
            var entity = await _context
                .ApplicationUsers
                .Include(u => u.SubscribedLocations)
                .FirstOrDefaultAsync(u => u.Id == user.Id);
            return entity.SubscribedLocations.ToList();
        }

        public async Task<IdentityResult> SaveSubscribedLocationsAsync(ApplicationUser user, IEnumerable<int> subscribedLocationsIds)
        {
            try
            {
                var entity = _context
                    .ApplicationUsers
                    .Include(u => u.SubscribedLocations)
                    .FirstOrDefault(u => u.Id == user.Id);
                
                if (entity == null) return IdentityResult.Failed();

                var subLocations = entity.SubscribedLocations.ToList();
                if (subLocations.Count != 0)
                {
                    foreach (var subLocation in subLocations)
                    {
                        var location = _context.Locations.First(l => l.Id == subLocation.Id);
                        _context.Entry(location).State = EntityState.Modified;
                        entity.SubscribedLocations.Remove(location);
                    }
                }

                if (subscribedLocationsIds != null)
                {
                    foreach (int id in subscribedLocationsIds)
                    {
                        var location = _context.Locations.First(l => l.Id == id);
                        if (location == null) continue;
                        _context.Entry(location).State = EntityState.Modified;
                        entity.SubscribedLocations.Add(location);
                    }
                }
                
                _context.Update(entity);
                await _context.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                //TODO logger
                return IdentityResult.Failed();
            }
        }

    }
}
