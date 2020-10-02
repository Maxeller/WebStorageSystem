using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Data.Entities.Products;

namespace WebStorageSystem.Data.Services.Products
{
    public class ProductTypeService
    {
        private readonly AppDbContext _context;
        //private readonly ProductService _pService;
        private readonly ILogger _logger;

        private IQueryable<ProductType> _getQuery;

        public ProductTypeService(AppDbContext context, /*ProductService pService,*/ ILoggerFactory factory)
        {
            _context = context;
            //_pService = pService;

            _logger = factory.CreateLogger<ProductTypeService>();

            _getQuery = _context
                .ProductTypes
                .AsNoTracking()
                .OrderBy(productType => productType.Name);
        }

        public async Task<ProductType> GetProductTypeAsync(int id, bool getDeleted = false)
        {
            /*var products = await _pService.GetProductsAsync(getDeleted);
            var myProducts = products.ToList().FindAll(product => product.ProductType.Id == id);*/
            ProductType productType = null;
            if (getDeleted)
            {
                productType = await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(productType => productType.Id == id);
                return productType;
            }

            productType = await _getQuery.FirstOrDefaultAsync(productType => productType.Id == id);
            return productType;
        }

        public async Task<ICollection<ProductType>> GetProductTypesAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        public async Task AddProductTypeAsync(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
        }

        public async Task<(bool Success, string ErrorMessage)> EditProductTypeAsync(ProductType productType)
        {
            try
            {
                _context.ProductTypes.Update(productType);
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

        public async Task<int> DeleteProductTypeAsync(int id)
        {
            var productType = await GetProductTypeAsync(id);
            return await DeleteProductTypeAsync(productType);
        }

        public async Task<int> DeleteProductTypeAsync(ProductType productType)
        {
            var pt = await GetProductTypeAsync(productType.Id);
            //if (pt.Products.Count() != 0) return -1;
            _context.ProductTypes.Remove(productType);
            return await _context.SaveChangesAsync();
        }

        public async Task RestoreProductTypeAsync(int id)
        {
            var productType = await GetProductTypeAsync(id, true);
            productType.IsDeleted = false;
            _context.Update(productType);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ProductTypeExistsAsync(int id, bool getDeleted)
        {
            var productType = await GetProductTypeAsync(id, getDeleted);
            return productType != null;
        }
    }
}
