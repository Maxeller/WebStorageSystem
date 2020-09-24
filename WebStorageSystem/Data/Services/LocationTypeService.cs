using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Data.Entities.Locations;

namespace WebStorageSystem.Data.Services
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

        public async Task<LocationType> GetLocationTypeAsync(int id, bool showDeleted = false)
        {
            if(showDeleted) return await _context.LocationTypes.IgnoreQueryFilters().FirstOrDefaultAsync(locationType => locationType.Id == id);
            return await _context.LocationTypes.FirstOrDefaultAsync(locationType => locationType.Id == id);
        }

        public async Task<ICollection<LocationType>> GetLocationTypesAsync(bool showDeleted = false)
        {
            if (showDeleted) return await _context.LocationTypes.IgnoreQueryFilters().ToListAsync();
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

        public async Task DeleteLocationType(int id)
        {
            var locationType = await GetLocationTypeAsync(id);
            await DeleteLocationType(locationType);
        }

        public async Task DeleteLocationType(LocationType locationType)
        {
            _context.LocationTypes.Remove(locationType);
            await _context.SaveChangesAsync();
        }

    }
}
