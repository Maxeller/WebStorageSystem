using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data;

namespace WebStorageSystem.Areas.Products.Data.Services
{
    public class VendorService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        private IQueryable<Vendor> _getQuery;

        public VendorService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;

            _logger = factory.CreateLogger<VendorService>();

            _getQuery = _context
                .Vendors
                .AsNoTracking()
                .OrderBy(vendor => vendor.Name);
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Vendor> GetVendorAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(vendor => vendor.Id == id);

            return await _getQuery.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Vendor>> GetVendorsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="vendor">Object for adding</param>
        public async Task AddVendorAsync(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="vendor">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditVendorAsync(Vendor vendor)
        {
            try
            {
                _context.Vendors.Update(vendor);
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
        public async Task DeleteVendorAsync(int id)
        {
            var vendor= await GetVendorAsync(id, true);
            await DeleteVendorAsync(vendor);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="unit">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task DeleteVendorAsync(Vendor vendor)
        {
            _context.Vendors.Remove(vendor); // TODO: Cascading
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreVendorAsync(int id)
        {
            var vendor = await GetVendorAsync(id, true);
            vendor.IsDeleted = false;
            _context.Update(vendor); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> VendorExistsAsync(int id, bool getDeleted)
        {
            var vendor = await GetVendorAsync(id, getDeleted);
            return vendor != null;
        }
    }
}
