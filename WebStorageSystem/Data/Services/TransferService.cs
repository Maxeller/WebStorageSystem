using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Infrastructure;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Models;

namespace WebStorageSystem.Data.Services
{
    public class TransferService
    {
        private readonly AppDbContext _context;
        private readonly IConfigurationProvider _mappingConfiguration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        private readonly IQueryable<Transfer> _getQuery;

        public TransferService(AppDbContext context, IConfigurationProvider mappingConfiguration, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILoggerFactory factory)
        {
            _context = context;
            _mappingConfiguration = mappingConfiguration;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = factory.CreateLogger<TransferService>();

            _getQuery = _context
                .Transfers.AsNoTracking()
                .OrderByDescending(transfer => transfer.ModifiedDate)
                .Include(transfer => transfer.OriginLocation).ThenInclude(location => location.LocationType).AsNoTracking()
                .Include(transfer => transfer.User).AsNoTracking()
                .Include(transfer => transfer.DestinationLocation).ThenInclude(location => location.LocationType).AsNoTracking()
                .Include(transfer => transfer.Units).ThenInclude(unit => unit.Product).ThenInclude(product => product.ProductType).AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Transfer> GetTransferAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(transfer => transfer.Id == id);
            return await _getQuery.FirstOrDefaultAsync(transfer => transfer.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Transfer>> GetTransfersAsync(bool getDeleted = false)
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
        public async Task<JqueryDataTablesPagedResults<TransferModel>> GetTransfersAsync(JqueryDataTablesParameters table, bool getDeleted = false)
        {
            TransferModel[] items;

            var query = _context
                .Transfers.AsNoTracking()
                .OrderByDescending(transfer => transfer.TransferTime)
                .Include(transfer => transfer.OriginLocation).ThenInclude(location => location.LocationType).AsNoTracking()
                .Include(transfer => transfer.DestinationLocation).ThenInclude(location => location.LocationType).AsNoTracking()
                .Include(transfer => transfer.User).AsNoTracking()
                .Include(transfer => transfer.Units).ThenInclude(unit => unit.Product).ThenInclude(product => product.ProductType).AsNoTracking();
                
            query = SearchOptionsProcessor<TransferModel, Transfer>.Apply(query, table.Columns);
            query = SortOptionsProcessor<TransferModel, Transfer>.Apply(query, table);

            var size = await query.CountAsync();

            if (table.Length > 0)
            {
                items = await query
                    .Skip((table.Start / table.Length) * table.Length)
                    .Take(table.Length)
                    .ProjectTo<TransferModel>(_mappingConfiguration)
                    .ToArrayAsync();
            }
            else
            {
                items = await query
                    .ProjectTo<TransferModel>(_mappingConfiguration)
                    .ToArrayAsync();
            }

            return new JqueryDataTablesPagedResults<TransferModel>
            {
                Items = items,
                TotalSize = size
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="transfer">Object for adding</param>
        public async Task AddTransferAsync(Transfer transfer)
        {
            var dbUnits = new List<Unit>();
            foreach (var unit in transfer.Units) // Moves all transferred units to destination location
            {
                var dbUnit = await _context.Units.FirstAsync(u => u.Id == unit.Id);
                dbUnit.LocationId = transfer.DestinationLocationId;
                _context.Entry(dbUnit).State = EntityState.Modified;
                _context.Units.Update(dbUnit);
                dbUnits.Add(dbUnit);
            }

            transfer.Units = dbUnits;
            //var transfers = await _context.Transfers.AsNoTracking().ToListAsync();
            //transfer.Id = transfers.Count == 0 ? 1 : transfers[^1].Id + 1;
            transfer.User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User); // Gets current user from HttpContext and sets it for this transfer
            transfer.TransferTime = DateTime.UtcNow; // Sets transfer time
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();
            Console.WriteLine(transfer.Id);
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="transfer">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditTransferAsync(Transfer transfer)
        {
            try
            {
                //var prev = await GetLocationAsync(transfer.Id);
                //_context.Entry(prev).State = EntityState.Detached;
                //_context.Entry(transfer).State = EntityState.Modified;
                _context.Transfers.Update(transfer);
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
        public async Task<(bool Success, string ErrorMessage)> DeleteTransferAsync(int id)
        {
            var location = await GetTransferAsync(id, true);
            return await DeleteTransferAsync(location);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="transfer">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteTransferAsync(Transfer transfer)
        {
            _context.Transfers.Remove(transfer); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreTransferAsync(int id)
        {
            var location = await GetTransferAsync(id, true);
            location.IsDeleted = false;
            _context.Update(location);
            await _context.SaveChangesAsync();
        }
    }
}