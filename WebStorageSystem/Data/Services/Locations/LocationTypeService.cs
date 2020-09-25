using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using WebStorageSystem.Data.Entities.Locations;

namespace WebStorageSystem.Data.Services.Locations
{
    public class LocationTypeService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public LocationTypeService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<LocationTypeService>();
        }

        public async Task<LocationType> GetLocationTypeAsync(int id, bool getDeleted = false)
        {
            if(getDeleted) return await _context.LocationTypes.IgnoreQueryFilters().FirstOrDefaultAsync(locationType => locationType.Id == id);
            return await _context.LocationTypes.FirstOrDefaultAsync(locationType => locationType.Id == id);
        }

        public async Task<ICollection<LocationType>> GetLocationTypesAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _context.LocationTypes.IgnoreQueryFilters().ToListAsync();
            return await _context.LocationTypes.ToListAsync();
        }

        public async Task AddLocationTypeAsync(LocationType locationType)
        {
            _context.LocationTypes.Add(locationType);
            await _context.SaveChangesAsync();
        }

        public async Task EditLocationTypeAsync(LocationType locationType)
        {
            _context.LocationTypes.Update(locationType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationTypeAsync(int id)
        {
            var locationType = await GetLocationTypeAsync(id);
            await DeleteLocationTypeAsync(locationType);
        }

        public async Task DeleteLocationTypeAsync(LocationType locationType)
        {
            _context.LocationTypes.Remove(locationType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LocationTypeExistsAsync(int id, bool getDeleted)
        {
            var locationType = await GetLocationTypeAsync(id, getDeleted);
            return locationType != null;
        }

        public async Task RestoreLocationType(int id)
        {
            var locationType = await GetLocationTypeAsync(id, true);
            locationType.IsDeleted = false;
            _context.Update(locationType);
            await _context.SaveChangesAsync();
        }
    }
}
