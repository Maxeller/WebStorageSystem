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
    public class BundleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<Bundle> _getQuery;

        public BundleService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<BundleService>();

            _getQuery = _context
                .Bundles
                .OrderBy(bundle => bundle.Name)
                .Include(bundle => bundle.BundledUnits)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.ProductType)
                .Include(bundle => bundle.BundledUnits)
                    .ThenInclude(unit => unit.Location)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Bundle> GetBundleAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(bundle => bundle.Id == id);
            return await _getQuery.FirstOrDefaultAsync(bundle => bundle.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Bundle>> GetBundlesAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<BundleModel>> GetBundlesAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .Bundles
                .Include(bundle => bundle.BundledUnits)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.ProductType)
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            // SEARCH USING IncludeFilter
            var list = await query.SpecialitySearchToList(request);

            var data =
                list.Select(bundle => _mapper.Map<BundleModel>(bundle)).AsParallel().ToArray();

            data = data.Where(bundle => bundle.NumberOfUnits > 0).ToArray();

            return new DataTableDbResult<BundleModel>
            {
                Data = data,
                RecordsTotal = data.Length
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="bundle">Object for adding</param>
        /// <param name="units">Objects where edited object is used</param>
        public async Task AddBundleAsync(Bundle bundle, IEnumerable<Unit> units)
        {
            _context.Bundles.Add(bundle);
            await _context.SaveChangesAsync();
            foreach (var unit in units)
            {
                var u = await _context.Units.FirstOrDefaultAsync(u => u.Id == unit.Id);
                _context.Entry(u).State = EntityState.Modified;
                u.PartOfBundle = _context.Bundles.Attach(bundle).Entity;
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="bundle">Object for editing</param>
        /// <param name="units">Objects where edited object is used</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditBundleAsync(Bundle bundle, IEnumerable<Unit> units)
        {
            try
            {
                var prev = await GetBundleAsync(bundle.Id);
                foreach (var bundledUnit in prev.BundledUnits)
                {
                    _context.Entry(bundledUnit).State = EntityState.Modified;
                    bundledUnit.PartOfBundleId = null;
                }
                foreach (var unit in units)
                {
                    var prevUnit = await _context.Units.FirstAsync(u => u.Id == unit.Id);
                    _context.Entry(prevUnit).State = EntityState.Detached;
                    _context.Entry(unit).State = EntityState.Modified;
                    unit.PartOfBundle = _context.Bundles.Attach(bundle).Entity;
                }

                _context.Bundles.Update(bundle);
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
        public async Task<(bool Success, string ErrorMessage)> DeleteBundleAsync(int id)
        {
            var bundle = await GetBundleAsync(id, true);
            return await DeleteBundleAsync(bundle);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="bundle">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteBundleAsync(Bundle bundle)
        {
            if (bundle.BundledUnits.Count(unit => !unit.IsDeleted) != 0) return (false, "Bundle cannot be deleted.<br/>Units used in bundle are in use.");
            _context.Bundles.Remove(bundle);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreBundleAsync(int id)
        {
            var bundle = await GetBundleAsync(id, true);
            bundle.IsDeleted = false;
            _context.Update(bundle);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> BundleExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) return await _context.Bundles.AsNoTracking().IgnoreQueryFilters().AnyAsync(bundle => bundle.Id == id);
            return await _context.Bundles.AsNoTracking().AnyAsync(bundle => bundle.Id == id);
        }
    }
}
