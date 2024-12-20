﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Locations.Data.Services
{
    public class LocationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<Location> _getQuery;

        public LocationService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<LocationService>();

            _getQuery = _context
                .Locations
                .OrderBy(location => location.Name)
                .Include(location => location.LocationType)
                .Include(location => location.Units)
                .Include(location => location.DefaultUnits)
                .Include(location => location.Bundles)
                .Include(location => location.DefaultBundles)
                .Include(location => location.OriginTransfers)
                .Include(location => location.DestinationTransfers)
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
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<LocationModel>> GetLocationsAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .Locations
                .Include(location => location.LocationType)
                .AsNoTracking()
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(location => _mapper.Map<LocationModel>(location)).AsParallel().ToArray();

            return new DataTableDbResult<LocationModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="location">Object for adding</param>
        public async Task AddLocationAsync(Location location)
        {
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
                location.CreatedDate = prev.CreatedDate;
                _context.Entry(location).State = EntityState.Modified;
                _context.Locations.Update(location);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                _logger.LogError(concurrencyException, concurrencyException.Message);
                return (false, concurrencyException.Message); // TODO: Change to more friendly message
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
            var location = await GetLocationAsync(id, true);
            return await DeleteLocationAsync(location);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="location">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteLocationAsync(Location location)
        {
            if (location.Units.Any(unit => !unit.IsDeleted) || location.DefaultUnits.Any(unit => !unit.IsDeleted)) return (false, "Location cannot be deleted.<br /> It's used as Location in existing Unit(s)");
            if (location.Bundles.Any(bundle => !bundle.IsDeleted) || location.DefaultBundles.Any(bundle => !bundle.IsDeleted)) return (false, "Location cannot be deleted.<br /> It's used as Location in existing Bundle(s)");
            if (location.OriginTransfers.Any() || location.DestinationTransfers.Any()) return (false, "Location cannot be deleted.<br /> It's used as Location in existing Transfer(s)");
            _context.Locations.Remove(location);
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
            if (getDeleted) return await _context.Locations.AsNoTracking().IgnoreQueryFilters().AnyAsync(location => location.Id == id);
            return await _context.Locations.AsNoTracking().AnyAsync(location => location.Id == id);
        }
    }
}