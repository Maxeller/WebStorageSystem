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

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Location> GetLocationAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(location => location.Id == id);
            return await _getQuery.FirstOrDefaultAsync(location => location.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Location>> GetLocationsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="location">Object for adding</param>
        public async Task AddLocationAsync(Location location)
        {
            location.LocationType = _context.LocationTypes.Attach(location.LocationType).Entity;
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="location">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
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

        /// <summary>
        /// Soft deletes entry based on entry ID
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteLocationAsync(int id)
        {
            var location = await GetLocationAsync(id);
            return await DeleteLocationAsync(location);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="location">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteLocationAsync(Location location)
        {
            _context.Locations.Remove(location); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreLocationAsync(int id)
        {
            var location = await GetLocationAsync(id, true);
            location.IsDeleted = false;
            _context.Update(location);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> LocationExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) await _context.Locations.AsNoTracking().IgnoreQueryFilters().AnyAsync(location => location.Id == id);
            return await _context.Locations.AsNoTracking().AnyAsync(location => location.Id == id);
        }
    }
}