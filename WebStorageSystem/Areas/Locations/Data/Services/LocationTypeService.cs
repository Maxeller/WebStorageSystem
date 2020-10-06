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
    public class LocationTypeService
    {
        private readonly AppDbContext _context;
        private readonly LocationService _lService;
        private readonly ILogger _logger;

        private readonly IQueryable<LocationType> _getQuery;

        public LocationTypeService(AppDbContext context, LocationService lService, ILoggerFactory factory)
        {
            _context = context;
            _lService = lService;

            _logger = factory.CreateLogger<LocationTypeService>();

            _getQuery = _context
                .LocationTypes
                .AsNoTracking()
                .OrderBy(locationType => locationType.Name)
                .Include(locationType => locationType.Locations);
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<LocationType> GetLocationTypeAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) await _getQuery.FirstOrDefaultAsync(locationType => locationType.Id == id);
            return await _getQuery.FirstOrDefaultAsync(locationType => locationType.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<LocationType>> GetLocationTypesAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="locationType">Object for adding</param>
        public async Task AddLocationTypeAsync(LocationType locationType)
        {
            _context.LocationTypes.Add(locationType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="locationType">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditLocationTypeAsync(LocationType locationType)
        {
            try
            {
                _context.LocationTypes.Update(locationType);
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
        /// <returns>Number of affected rows (return -1 when entry is used as foreign key)</returns>
        public async Task<int> DeleteLocationTypeAsync(int id)
        {
            var locationType = await GetLocationTypeAsync(id);
            return await DeleteLocationTypeAsync(locationType);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="locationType">Object for deletion</param>
        /// <returns>Number of affected rows (return -1 when entry is used as foreign key)</returns>
        public async Task<int> DeleteLocationTypeAsync(LocationType locationType)
        {
            var n = locationType.Locations.Count(location => !location.IsDeleted);
            if (n != 0) return -1;
            _context.LocationTypes.Remove(locationType);
            return await _context.SaveChangesAsync();
        }

        /// /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreLocationTypeAsync(int id)
        {
            var locationType = await GetLocationTypeAsync(id, true);
            locationType.IsDeleted = false;
            _context.Update(locationType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> LocationTypeExistsAsync(int id, bool getDeleted)
        {
            var locationType = await GetLocationTypeAsync(id, getDeleted);
            return locationType != null;
        }
    }
}
