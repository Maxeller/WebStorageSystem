﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data;

namespace WebStorageSystem.Areas.Products.Data.Services
{
    public class ManufacturerService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationProvider _mappingConfiguration;
        private readonly ILogger _logger;

        private readonly IQueryable<Manufacturer> _getQuery;

        public ManufacturerService(AppDbContext context, IConfigurationProvider mappingConfiguration, ILoggerFactory factory)
        {
            _context = context;
            _mappingConfiguration = mappingConfiguration;
            _logger = factory.CreateLogger<ManufacturerService>();

            _getQuery = _context
                .Manufacturers
                .AsNoTracking()
                .OrderBy(manufacturer => manufacturer.Name)
                .Include(manufacturer => manufacturer.Products)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Manufacturer> GetManufacturerAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);

            return await _getQuery.FirstOrDefaultAsync(manufacturer => manufacturer.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Manufacturer>> GetManufacturersAsync(bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().ToListAsync();
            return await _getQuery.ToListAsync();
        }

        /// <summary>
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="table">JqueryDataTablesParameters with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>JqueryDataTablesPagedResults</returns>
        public async Task<JqueryDataTablesPagedResults<ManufacturerModel>> GetManufacturersAsync(JqueryDataTablesParameters table, bool getDeleted = false)
        {
            ManufacturerModel[] items;

            var query = _context
                .Manufacturers
                .AsNoTracking()
                .IgnoreQueryFilters();


            query = SearchOptionsProcessor<ManufacturerModel, Manufacturer>.Apply(query, table.Columns);
            query = SortOptionsProcessor<ManufacturerModel, Manufacturer>.Apply(query, table);

            var size = await query.CountAsync();

            if (table.Length > 0)
            {
                items = await query
                    .Skip((table.Start / table.Length) * table.Length)
                    .Take(table.Length)
                    .ProjectTo<ManufacturerModel>(_mappingConfiguration)
                    .ToArrayAsync();
            }
            else
            {
                items = await query
                    .ProjectTo<ManufacturerModel>(_mappingConfiguration)
                    .ToArrayAsync();
            }

            return new JqueryDataTablesPagedResults<ManufacturerModel>
            {
                Items = items,
                TotalSize = size
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="manufacturer">Object for adding</param>
        public async Task AddManufacturerAsync(Manufacturer manufacturer)
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="manufacturer">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
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

        /// <summary>
        /// Soft deletes entry based on entry ID
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteManufacturerAsync(int id)
        {
            var manufacturer = await GetManufacturerAsync(id, true);
            return await DeleteManufacturerAsync(manufacturer);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="manufacturer">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteManufacturerAsync(Manufacturer manufacturer)
        {
            if (manufacturer.Products.Count(product => !product.IsDeleted) != 0) return (false, "Manufacturer cannot be deleted.<br/>It's used in existing Product.");
            _context.Manufacturers.Remove(manufacturer);
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreManufacturerAsync(int id)
        {
            var manufacturer = await GetManufacturerAsync(id, true);
            manufacturer.IsDeleted = false;
            _context.Update(manufacturer);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> ManufacturerExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted)
                return await _context.Manufacturers.AsNoTracking().IgnoreQueryFilters()
                    .AnyAsync(manufacturer => manufacturer.Id == id);
            return await _context.Manufacturers.AsNoTracking().AnyAsync(manufacturer => manufacturer.Id == id);
        }
    }
}
