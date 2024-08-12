using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Defects.Models;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Defects.Data.Services
{
    public class DefectService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IQueryable<Defect> _getQuery;

        public DefectService(AppDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILoggerFactory factory)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _logger = factory.CreateLogger<DefectService>();

            _getQuery = _context
                .Defects
                .OrderBy(defect => defect.ModifiedDate)
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.ProductType)
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.Manufacturer)
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Location)
                        .ThenInclude(location => location.LocationType)
                .Include(defect => defect.ReportedByUser)
                .Include(defect => defect.CausedByUser)
                .Include(defect => defect.Image)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<Defect> GetDefectAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(defect => defect.Id == id);
            return await _getQuery.FirstOrDefaultAsync(defect => defect.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<Defect>> GetDefectsAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<DefectModel>> GetDefectsAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .Defects
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Product)
                    .ThenInclude(product => product.ProductType)
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.Manufacturer)
                .Include(defect => defect.Unit)
                    .ThenInclude(unit => unit.Location)
                        //.ThenInclude(location => location.LocationType)
                .Include(defect => defect.ReportedByUser)
                //.Include(defect => defect.CausedByUser)
                .AsNoTracking();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(defect => _mapper.Map<DefectModel>(defect)).AsParallel().ToArray();

            return new DataTableDbResult<DefectModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="defect">Object for adding</param>
        public async Task AddDefectAsync(Defect defect)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User); // Gets current user from HttpContext
            defect.ReportedByUserId = currentUser.Id;
            _context.Defects.Add(defect);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Edits entry in DB
        /// </summary>
        /// <param name="defect">Object for editing</param>
        /// <returns>Return tuple if editing was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> EditDefectAsync(Defect defect)
        {
            try
            {
                var prev = await _context.Defects.FirstAsync(d => d.Id == defect.Id);
                _context.Entry(prev).State = EntityState.Detached;
                _context.Entry(defect).State = EntityState.Modified;
                _context.Defects.Update(defect);
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
        public async Task<(bool Success, string ErrorMessage)> DeleteDefectAsync(int id)
        {
            var defect = await GetDefectAsync(id, true);
            return await DeleteDefectAsync(defect);
        }

        /// <summary>
        /// Soft deletes entry based on object
        /// </summary>
        /// <param name="defect">Object for deletion</param>
        /// <returns>Return tuple if deleting was successful, if not error message is provided</returns>
        public async Task<(bool Success, string ErrorMessage)> DeleteDefectAsync(Defect defect)
        {
            _context.Defects.Remove(defect); // TODO: Determine if cascading
            await _context.SaveChangesAsync();
            return (true, null);
        }

        /// <summary>
        /// Restores soft deleted entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        public async Task RestoreDefectAsync(int id)
        {
            var defect = await GetDefectAsync(id, true);
            defect.IsDeleted = false;
            _context.Update(defect);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Determines existence of entry
        /// </summary>
        /// <param name="id">Entry ID</param>
        /// <param name="getDeleted">Look through soft deleted entries</param>
        /// <returns>True if entry exists</returns>
        public async Task<bool> DefectExistsAsync(int id, bool getDeleted)
        {
            if (getDeleted) await _context.Defects.AsNoTracking().IgnoreQueryFilters().AnyAsync(defect => defect.Id == id);
            return await _context.Defects.AsNoTracking().AnyAsync(defect => defect.Id == id);
        }
    }
}
