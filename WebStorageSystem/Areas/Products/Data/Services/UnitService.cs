using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Data.Services
{
    public class UnitService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<Unit> _getQuery;

        public UnitService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<ProductService>();

            _getQuery = _context.Units
                .Include(unit => unit.Product)
                    .ThenInclude(product => product.ProductType)
                .Include(unit => unit.Product)
                    .ThenInclude(product => product.Manufacturer)
                .Include(unit => unit.Location)
                    .ThenInclude(location => location.LocationType)
                .Include(unit => unit.DefaultLocation)
                    .ThenInclude(location => location.LocationType)
                .Include(unit => unit.Vendor)
                .Include(unit => unit.PartOfBundle)
                .AsNoTracking()
                .IgnoreQueryFilters();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Unit> GetUnitAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(unit => unit.Id == id);
            return await _getQuery.FirstOrDefaultAsync(unit => unit.Id == id);
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="ids">Entity IDs</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<IEnumerable<Unit>> GetUnitsAsync(IEnumerable<int> ids, bool getDeleted = false)
        {
            var result = new List<Unit>();
            foreach (var id in ids)
            {
                if (getDeleted)
                {
                    result.Add(await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(unit => unit.Id == id));
                }
                else
                {
                    result.Add(await _getQuery.FirstOrDefaultAsync(unit => unit.Id == id));
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Unit>> GetUnitsAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<UnitModel>> GetUnitsAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .Units
                .Include(unit => unit.Product)
                    .ThenInclude(product => product.ProductType)
                .Include(unit => unit.Product)
                    .ThenInclude(product => product.Manufacturer)
                .Include(unit => unit.Location)
                .Include(unit => unit.DefaultLocation)
                .Include(unit => unit.Vendor)
                .Include(unit => unit.PartOfBundle)
                .AsNoTracking()
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(unit => _mapper.Map<UnitModel>(unit)).AsParallel().ToArray();

            return new DataTableDbResult<UnitModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="unit">Object for adding</param>
        public async Task AddUnitAsync(Unit unit)
        {
            unit.Product = _context.Products.Attach(unit.Product).Entity;
            unit.Location = _context.Locations.Attach(unit.Location).Entity;
            if (unit.Vendor != null) unit.Vendor = _context.Vendors.Attach(unit.Vendor).Entity;
            if (unit.PartOfBundle != null) unit.PartOfBundle = _context.Bundles.Attach(unit.PartOfBundle).Entity;
            unit.LastCheckTime = DateTime.Now;
            _context.Units.Add(unit);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="unit">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditUnitAsync(Unit unit)
        {
            try
            {
                var prev = await _context.Units.FirstAsync(u => u.Id == unit.Id);
                _context.Entry(prev).State = EntityState.Detached;
                _context.Entry(unit).State = EntityState.Modified;

                _context.Units.Update(unit);
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
        public async Task<(bool Success, string ErrorMessage)> DeleteUnitAsync(int id)
        {
            var unit = await GetUnitAsync(id, true);
            return await DeleteUnitAsync(unit);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="unit">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteUnitAsync(Unit unit)
        {
            _context.Units.Remove(unit); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreUnitAsync(int id)
        {
            var unit = await GetUnitAsync(id, true);
            unit.IsDeleted = false;
            _context.Update(unit);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> UnitExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) await _context.Units.AsNoTracking().IgnoreQueryFilters().AnyAsync(unit => unit.Id == id);
            return await _context.Units.AsNoTracking().AnyAsync(unit => unit.Id == id);
        }
    }
}
