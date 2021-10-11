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
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<Product> _getQuery;

        public ProductService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<ProductService>();

            _getQuery = _context
                .Products
                .OrderBy(product => product.Name)
                .Include(product => product.ProductType)
                .Include(product => product.Manufacturer)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Product> GetProductAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(product => product.Id == id);
            return await _getQuery.FirstOrDefaultAsync(product => product.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<ProductModel>> GetProductsAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .Products
                .OrderBy(product => product.Name)
                .Include(product => product.ProductType)
                .Include(product => product.Manufacturer)
                .AsNoTracking()
                .IgnoreQueryFilters();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(product => _mapper.Map<ProductModel>(product)).AsParallel().ToArray();

            return new DataTableDbResult<ProductModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="product">Object for adding</param>
        public async Task AddProductAsync(Product product)
        {
            product.Manufacturer = _context.Manufacturers.Attach(product.Manufacturer).Entity;
            product.ProductType = _context.ProductTypes.Attach(product.ProductType).Entity;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="product">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditProductAsync(Product product)
        {
            try
            {
                var prev = await _context.Products.FirstAsync(p => p.Id == product.Id);
                _context.Entry(prev).State = EntityState.Detached;
                _context.Entry(product).State = EntityState.Modified;

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

        /// <summary>
        /// Soft deletes entry based on entry ID
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteProductAsync(int id)
        {
            var product = await GetProductAsync(id, true);
            return await DeleteProductAsync(product);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="product">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreProductAsync(int id)
        {
            var product = await GetProductAsync(id, true);
            product.IsDeleted = false;
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> ProductExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) await _context.Products.AsNoTracking().IgnoreQueryFilters().AnyAsync(product => product.Id == id);
            return await _context.Products.AsNoTracking().AnyAsync(product => product.Id == id);
        }
    }
}