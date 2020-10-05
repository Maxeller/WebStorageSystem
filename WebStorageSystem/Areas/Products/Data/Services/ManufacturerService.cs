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
    public class ManufacturerService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        private IQueryable<Manufacturer> _getQuery;

        public ManufacturerService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;

            _logger = factory.CreateLogger<ManufacturerService>();

            _getQuery = _context
                .Manufacturers
                .AsNoTracking()
                .OrderBy(manufacturer => manufacturer.Name);
        }

        public async Task<Manufacturer> GetManufacturerAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);

            return await _getQuery.FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturersAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> EditManufacturerAsync(Manufacturer manufacturer)
        {
            try
            {
                _context.Manufacturers.Update(manufacturer);
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

        public async Task DeleteManufacturerAsync(int id)
        {
            var manufacturer= await GetManufacturerAsync(id);
            await DeleteManufacturerAsync(manufacturer);
        }

        public async Task DeleteManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Manufacturers.Remove(manufacturer); // TODO: Cascading 
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ManufacturerExistsAsync(int id, bool getDeleted)
        {
            var manufacturer = await GetManufacturerAsync(id, getDeleted);
            return manufacturer != null;
        }

        public async Task RestoreManufacturerAsync(int id)
        {
            var manufacturer = await GetManufacturerAsync(id, true);
            manufacturer.IsDeleted = false;
            _context.Update(manufacturer); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }
    }
}
