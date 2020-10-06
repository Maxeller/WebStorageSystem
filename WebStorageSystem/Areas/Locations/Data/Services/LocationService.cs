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
            _logger = factory.CreateLogger<LocationService>();

            _getQuery = _context
                .Locations
                .AsNoTracking()
                .OrderBy(location => location.Name)
                .Include(location => location.LocationType)
                .AsNoTracking();
        }

        public async Task<Location> GetLocationAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(location => location.Id == id);
            return await _getQuery.FirstOrDefaultAsync(location => location.Id == id);
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddLocationAsync(Location location)
        {
            location.LocationType = _context.LocationTypes.Attach(location.LocationType).Entity;
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> EditLocationAsync(Location location)
        {
            try
            {
                var prev = await _context.Locations.FirstAsync(l => l.Id == location.Id);
                _context.Entry(prev).State = EntityState.Detached;
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
            var location = await GetLocationAsync(id);
            await DeleteLocationAsync(location);
        }

        public async Task DeleteLocationAsync(Location location)
        {
            _context.Locations.Remove(location); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }

        public async Task<bool> LocationExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) await _context.Locations.IgnoreQueryFilters().AnyAsync(location => location.Id == id);
            return await _context.Locations.AnyAsync(location => location.Id == id);
        }

        public async Task RestoreLocationAsync(int id)
        {
            var location = await GetLocationAsync(id, true);
            location.IsDeleted = false;
            _context.Update(location);
            await _context.SaveChangesAsync();
        }
    }
}