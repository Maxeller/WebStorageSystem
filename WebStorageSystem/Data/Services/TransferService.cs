using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Extensions;
using WebStorageSystem.Models.DataTables;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Data.Services
{
    public class TransferService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger _logger;

        private readonly IQueryable<MainTransfer> _getQuery;
        
        public TransferService(AppDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = factory.CreateLogger<TransferService>();

            _getQuery = _context
                .MainTransfers
                .OrderByDescending(mainTransfer => mainTransfer.TransferTime)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.OriginLocation)
                        .ThenInclude(originLocation => originLocation.LocationType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.DestinationLocation)
                        .ThenInclude(destinationLocation => destinationLocation.LocationType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.Unit)
                        .ThenInclude(unit => unit.Product)
                            .ThenInclude(product => product.ProductType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.Unit)
                        .ThenInclude(unit => unit.Vendor)
                .Include(mainTransfer => mainTransfer.User);
        }

        /// <summary>
        /// Gets entry from DB
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>If found returns object, otherwise null</returns>
        public async Task<MainTransfer> GetTransferAsync(int id, bool getDeleted = false)
        {
            if (getDeleted) return await _getQuery.IgnoreQueryFilters().FirstOrDefaultAsync(transfer => transfer.Id == id);
            return await _getQuery.FirstOrDefaultAsync(transfer => transfer.Id == id);
        }

        /// <summary>
        /// Gets all entries from DB
        /// </summary>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>Collection of entities</returns>
        public async Task<IEnumerable<MainTransfer>> GetTransfersAsync(bool getDeleted = false)
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
        public async Task<DataTableDbResult<SubTransferModel>> GetTransfersAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .SubTransfers
                .Include(subTransfer => subTransfer.MainTransfer)
                    .ThenInclude(mainTransfer => mainTransfer.User)
                .Include(subTransfer => subTransfer.OriginLocation)
                    .ThenInclude(location => location.LocationType)
                .Include(st => st.DestinationLocation)
                    .ThenInclude(location => location.LocationType)
                .Include(st => st.Unit)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.ProductType)
                .Include(subTransfer => subTransfer.Unit)
                    .ThenInclude(unit => unit.Vendor)
                .AsNoTracking();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(transfer => _mapper.Map<SubTransferModel>(transfer)).AsParallel().ToArray();

            return new DataTableDbResult<SubTransferModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="transfer">Object for adding</param>
        public async Task AddTransferAsync(MainTransfer transfer)
        {
            /*
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

            transfer.User = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User); // Gets current user from HttpContext and sets it for this transfer
            transfer.TransferTime = DateTime.UtcNow; // Sets transfer time
            _context.Transfers.Add(transfer);
            await _context.SaveChangesAsync();
            */
        }

        /*
        /// <summary>
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<MainTransferModel>> GetTransfersAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .MainTransfers
                .OrderByDescending(mainTransfer => mainTransfer.TransferTime)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.OriginLocation)
                        //.ThenInclude(originLocation => originLocation.LocationType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.DestinationLocation)
                        //.ThenInclude(destinationLocation => destinationLocation.LocationType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.Unit)
                        .ThenInclude(unit => unit.Product)
                            .ThenInclude(product => product.ProductType)
                .Include(mainTransfer => mainTransfer.SubTransfers)
                    .ThenInclude(subTransfer => subTransfer.Unit)
                        .ThenInclude(unit => unit.Vendor)
                .Include(mainTransfer => mainTransfer.User)
                .AsNoTracking();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            var count = await query.CountAsync();

            var data =
                query.Select(transfer => _mapper.Map<MainTransferModel>(transfer)).AsParallel().ToArray();

            return new DataTableDbResult<MainTransferModel>
            {
                Data = data,
                RecordsTotal = count
            };
        }
        */
    }
}