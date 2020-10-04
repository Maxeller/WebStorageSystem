using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Data;

namespace WebStorageSystem.Areas.Locations.Data.Services
{
    public class LocationService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        private readonly IQueryable<Location> _getQuery;

        public LocationService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<LocationTypeService>();

            _getQuery = _context
                .Locations
                .AsNoTracking()
                .OrderBy(location => location.Name)
                .Join(_context.LocationTypes,
                    location => location.LocationType.Id,
                    type => type.Id,
                    (location, locationType) => new Location(location, locationType));
        }

        public async Task<Location> GetLocationAsync(int id, bool getDeleted = false)
        {
            if (getDeleted)
            {
                var listWithDeleted = await _getQuery.IgnoreQueryFilters().ToListAsync();
                return listWithDeleted.FirstOrDefault(location => location.Id == id);
            }
            var list = await _getQuery.ToListAsync();
            return list.FirstOrDefault(location => location.Id == id);
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddLocationAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> EditLocationAsync(Location location)
        {
            try
            {
                var entry = _context.Locations.First(l => l.Id == location.Id);
                _context.Entry(entry).State = EntityState.Detached; // Because location was created via join (i think), previous version of it is still attached in context. It must be detached, so updated one can be properly saved.
                _context.Entry(location).State = EntityState.Modified;
                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                // TODO: log
                return (false, concurrencyException.Message); // TODO: Change to more friendly message
            }
            catch (Exception ex)
            {
                // TODO: log
                return (false, ex.Message); // TODO: Change to more friendly message
            }
        }

        public async Task DeleteLocationAsync(int id)
        {
            var locationType = await GetLocationAsync(id);
            await DeleteLocationAsync(locationType);
        }

        public async Task DeleteLocationAsync(Location location)
        {
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LocationExistsAsync(int id, bool getDeleted)
        {
            var location = await GetLocationAsync(id, getDeleted);
            return location != null;
        }

        public async Task RestoreLocationAsync(int id)
        {
            var location = await GetLocationAsync(id, true);
            location.IsDeleted = false;
            _context.Update(location); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }
    }
}
