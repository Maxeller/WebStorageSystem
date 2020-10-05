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
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        private IQueryable<Product> _getQuery;

        public ProductService(AppDbContext context, ILoggerFactory factory)
        {
            _context = context;
            _logger = factory.CreateLogger<ProductService>();

            _getQuery = _context.Products
                .AsNoTracking()
                .OrderBy(product => product.Name)
                .Include(product => product.ProductType)
                .Include(product => product.Manufacturer);
        }

        public async Task<Product> GetProductAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(product => product.Id == id);
            return await _getQuery.FirstOrDefaultAsync(product => product.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            product.Manufacturer = _context.Manufacturers.Attach(product.Manufacturer).Entity;
            product.ProductType = _context.ProductTypes.Attach(product.ProductType).Entity;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> EditProductAsync(Product product)
        {
            try
            {
                _context.Products.Update(product);
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

        public async Task DeleteProductAsync(int id)
        {
            var product = await GetProductAsync(id);
            await DeleteLocationAsync(product);
        }

        public async Task DeleteLocationAsync(Product product)
        {
            _context.Products.Remove(product); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductExistsAsync(int id, bool getDeleted)
        {
            var product = await GetProductAsync(id, getDeleted);
            return product != null;
        }

        public async Task RestoreProductAsync(int id)
        {
            var product = await GetProductAsync(id, true);
            product.IsDeleted = false;
            _context.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
