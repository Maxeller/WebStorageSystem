using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Locations.Data.Services
{
    public class LocationTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<LocationType> _getQuery;

        public LocationTypeService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<LocationTypeService>();

            _getQuery = _context
                .LocationTypes
                .OrderBy(locationType => locationType.Name)
                .Include(locationType => locationType.Locations)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<LocationType> GetLocationTypeAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(locationType => locationType.Id == id);
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
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<LocationTypeModel>> GetLocationTypesAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .LocationTypes
                .AsNoTracking()
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(locationType => _mapper.Map<LocationTypeModel>(locationType)).AsParallel().ToArray();

            return new DataTableDbResult<LocationTypeModel>
            {
                Data = data,
                RecordsTotal = count
            };
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
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteLocationTypeAsync(int id)
        {
            var locationType = await GetLocationTypeAsync(id, true);
            return locationType == null ? (false, $"Entry with ID {id} not found") : await DeleteLocationTypeAsync(locationType);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="locationType">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteLocationTypeAsync(LocationType locationType)
        {
            if (locationType.Locations.Any(location => !location.IsDeleted)) return (false, "Location Type cannot be deleted.<br/>It's used as type in existing Location.");
            _context.LocationTypes.Remove(locationType);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
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
            if (getDeleted) return await _context.LocationTypes.AsNoTracking().IgnoreQueryFilters().AnyAsync(locationType => locationType.Id == id);
            return await _context.LocationTypes.AsNoTracking().AnyAsync(locationType => locationType.Id == id);
        }
    }
}
