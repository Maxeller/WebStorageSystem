using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Data.Services
{
    public class ProductTypeService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationProvider _mappingConfiguration;
        private readonly ILogger _logger;

        private IQueryable<ProductType> _getQuery;

        public ProductTypeService(AppDbContext context, IConfigurationProvider mappingConfiguration, ILoggerFactory factory)
        {
            _context = context;
            _mappingConfiguration = mappingConfiguration;
            _logger = factory.CreateLogger<ProductTypeService>();

            _getQuery = _context
                .ProductTypes
                .AsNoTracking()
                .OrderBy(productType => productType.Name)
                .Include(productType => productType.Products)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<ProductType> GetProductTypeAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(productType => productType.Id == id);
            return await _getQuery.FirstOrDefaultAsync(productType => productType.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<ProductType>> GetProductTypesAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<ProductTypeModel>> GetProductTypesAsync(DataTableRequest request, bool getDeleted = false)
        {
            ProductTypeModel[] data;

            var query = _context
                .ProductTypes
                .AsNoTracking()
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            data = await query
                .ProjectTo<ProductTypeModel>(_mappingConfiguration)
                .ToArrayAsync();

            return new DataTableDbResult<ProductTypeModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="productType">Object for adding</param>
        public async Task AddProductTypeAsync(ProductType productType)
        {
            _context.ProductTypes.Add(productType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="productType">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
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

        /// <summary>
        /// Soft deletes entry based on entry ID
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteProductTypeAsync(int id)
        {
            var productType = await GetProductTypeAsync(id, true);
            return await DeleteProductTypeAsync(productType);
        }


        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="productType">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteProductTypeAsync(ProductType productType)
        {
            if (productType.Products.Count(product => !product.IsDeleted) != 0) return (false, "Product Type cannot be deleted.<br/>It's used as type in existing Product.");
            _context.ProductTypes.Remove(productType);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreProductTypeAsync(int id)
        {
            var productType = await GetProductTypeAsync(id, true);
            productType.IsDeleted = false;
            _context.Update(productType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> ProductTypeExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) return await _context.ProductTypes.AsNoTracking().IgnoreQueryFilters().AnyAsync(productType => productType.Id == id);
            return await _context.ProductTypes.AsNoTracking().AnyAsync(productType => productType.Id == id);
        }
    }
}
