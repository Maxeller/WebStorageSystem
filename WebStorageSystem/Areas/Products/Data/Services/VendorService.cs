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

        public async Task<Vendor> GetVendorAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(vendor => vendor.Id == id);

            return await _getQuery.FirstOrDefaultAsync(vendor => vendor.Id == id);
        }

        public async Task<IEnumerable<Vendor>> GetVendorsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddVendorAsync(Vendor vendor)
        {
            _context.Vendors.Add(vendor);
            await _context.SaveChangesAsync();
        }

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

        public async Task DeleteVendorAsync(int id)
        {
            var vendor= await GetVendorAsync(id);
            await DeleteVendorAsync(vendor);
        }

        public async Task DeleteVendorAsync(Vendor vendor)
        {
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VendorExistsAsync(int id, bool getDeleted)
        {
            var vendor = await GetVendorAsync(id, getDeleted);
            return vendor != null;
        }

        public async Task RestoreVendorAsync(int id)
        {
            var vendor = await GetVendorAsync(id, true);
            vendor.IsDeleted = false;
            _context.Update(vendor); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }
    }
}
