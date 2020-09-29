using System.Collections.Generic;
using System.Linq;
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
        private readonly LocationService _lService;
        private readonly ILogger _logger;

        private IQueryable<LocationType> _getQuery;

        public LocationTypeService(AppDbContext context, LocationService lService, ILoggerFactory factory)
        {
            _context = context;
            _lService = lService;

            _logger = factory.CreateLogger<LocationTypeService>();

            _getQuery = _context
                .LocationTypes
                .AsNoTracking()
                .OrderBy(locationType => locationType.Name);
        }

        public async Task<LocationType> GetLocationTypeAsync(int id, bool getDeleted = false)
        {
            var locations = await _lService.GetLocationsAsync(getDeleted);
            var myLocations = locations.ToList().FindAll(location => location.LocationType.Id == id);
            LocationType locationType = null;
            if (getDeleted)
            {
                locationType = await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(locationType => locationType.Id == id);
                locationType.Locations = myLocations;
                return locationType;
            }
            
            locationType = await _getQuery.FirstOrDefaultAsync(locationType => locationType.Id == id);
            locationType.Locations = myLocations;
            return locationType;
        }

        public async Task<ICollection<LocationType>> GetLocationTypesAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
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
            _context.Update(locationType); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }
    }
}
