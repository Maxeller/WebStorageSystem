using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Products.Models;
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
        public async Task<DataTableDbResult<SubTransferModel>> GetSubTransfersAsync(DataTableRequest request, bool getDeleted = false)
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
                .Include(subTransfer => subTransfer.Bundle)
                .AsNoTracking();

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            // SPECIALTY SEARCH
            query = query.SpecialitySearch(request);

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
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<SubTransferModel>> GetSubTransfersForDetailAsync(DataTableRequest request, bool getDeleted = false)
        {
            var query = _context
                .SubTransfers
                .Where(st => st.MainTransferId == int.Parse(request.AdditionalData.MainTransferId))
                .Include(st => st.Bundle)
                .ThenInclude(b => b.BundledUnits)
                .ThenInclude(u => u.Product)
                .Include(st => st.Unit)
                .ThenInclude(u => u.Product)
                .Include(st => st.OriginLocation)
                .AsNoTracking();


            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            // SPECIALTY SEARCH
            query = query.SpecialitySearch(request);

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
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<UnitBundleViewModel>> GetUnitBundleViewAsync(DataTableRequest request)
        {
            var query = _context
                .UnitsBundleView
                .Include(view => view.Location)
                    .ThenInclude(location => location.LocationType)
                .Include(view => view.DefaultLocation)
                    .ThenInclude(location => location.LocationType)
                .Include(view => view.Unit)
                    .ThenInclude(unit => unit.Product)
                        .ThenInclude(product => product.ProductType)
                .Include(view => view.Bundle)
                    .ThenInclude(bundle => bundle.BundledUnits)
                        .ThenInclude(unit => unit.Product)
                            .ThenInclude(product => product.ProductType)
                .AsNoTracking();
                

            // SEARCH
            query = query.Search(request);

            // ORDER
            query = query.OrderBy(request);

            // SPECIALTY SEARCH
            var list = await query.SpecialitySearchToList(request);

            var data = list.Select(view => _mapper.Map<UnitBundleViewModel>(view)).AsParallel().ToArray();

            data = data.Where(view => view.IsBundle && view.Bundle.NumberOfUnits > 0 || !view.IsBundle).ToArray();

            return new DataTableDbResult<UnitBundleViewModel>
            {
                Data = data,
                RecordsTotal = data.Length
            };
        }

        /// <summary>
        /// Adds object to DB
        /// </summary>
        /// <param name="mainTransfer">Object for adding</param>
        /// <param name="selectedUnitBundle">Selected Units/Bundles to mainTransfer</param>
        public async Task AddTransferAsync(MainTransfer mainTransfer, List<UnitBundleViewModel> selectedUnitBundle)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            mainTransfer.UserId = currentUser.Id;
            if (mainTransfer.State == TransferState.Transferred) mainTransfer.TransferTime = DateTime.UtcNow;
            var row = _context.MainTransfers.Add(mainTransfer);
            await _context.SaveChangesAsync();

            foreach (var ub in selectedUnitBundle)
            {
                var subTransfer = new SubTransfer
                {
                    MainTransferId = row.Entity.Id,
                    DestinationLocationId = row.Entity.DestinationLocationId
                };

                if (ub.IsBundle)
                {
                    var bundle = await _context.Bundles
                        .Include(b => b.BundledUnits)
                        .FirstOrDefaultAsync(b => b.Id == ub.BundleId);
                    subTransfer.BundleId = bundle.Id;
                    subTransfer.OriginLocationId = bundle.LocationId;
                    
                    if (row.Entity.State == TransferState.Transferred)
                    {
                        _context.Entry(bundle).State = EntityState.Modified;
                        bundle.LocationId = subTransfer.DestinationLocationId;
                        _context.Bundles.Update(bundle);

                        foreach (var bundledUnit in bundle.BundledUnits)
                        {
                            var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == bundledUnit.Id);
                            _context.Entry(unit).State = EntityState.Modified;
                            unit.LastTransferTime = DateTime.UtcNow;
                            unit.LocationId = subTransfer.DestinationLocationId;
                            _context.Units.Update(unit);
                        }
                    }
                }
                else
                {
                    var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == ub.UnitId);
                    subTransfer.UnitId = unit.Id;
                    subTransfer.OriginLocationId = unit.LocationId;
                    if (row.Entity.State == TransferState.Transferred)
                    {
                        _context.Entry(unit).State = EntityState.Modified;
                        unit.LastTransferTime = DateTime.UtcNow;
                        unit.LocationId = subTransfer.DestinationLocationId;
                        _context.Units.Update(unit);
                    }
                }

                _context.SubTransfers.Add(subTransfer);
            }
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Transfer Units/Bundles from Prepared Transfer
        /// </summary>
        /// <param name="mainTransferId">ID of MainTransfer</param>
        /// <returns></returns>
        public async Task<(bool Success, string ErrorMessage)> Transfer(int mainTransferId)
        {
            try
            {
                var mainTransfer = await _context.MainTransfers.FirstOrDefaultAsync(mt => mt.Id == mainTransferId);
                if (mainTransfer is { State: TransferState.Prepared })
                {
                    _context.Entry(mainTransfer).State = EntityState.Modified;
                    mainTransfer.State = TransferState.Transferred;
                    mainTransfer.TransferTime = DateTime.UtcNow;
                    _context.MainTransfers.Update(mainTransfer);

                    var subTransfers = await _context.SubTransfers.Where(st => st.MainTransferId == mainTransferId).ToListAsync();
                    foreach (var subTransfer in subTransfers)
                    {
                        if (subTransfer.BundleId == null)
                        {
                            var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == subTransfer.UnitId);
                            _context.Entry(unit).State = EntityState.Modified;
                            unit.LastTransferTime = DateTime.UtcNow;
                            unit.LocationId = subTransfer.DestinationLocationId;
                            _context.Units.Update(unit);
                        }
                        else
                        {
                            var bundle = await _context.Bundles
                                .Include(b => b.BundledUnits)
                                .FirstOrDefaultAsync(b => b.Id == subTransfer.BundleId);
                            _context.Entry(bundle).State = EntityState.Modified;
                            _context.Bundles.Update(bundle);
                            foreach (var bundledUnit in bundle.BundledUnits)
                            {
                                var unit = await _context.Units.FirstOrDefaultAsync(u => u.Id == bundledUnit.Id);
                                _context.Entry(unit).State = EntityState.Modified;
                                unit.LastTransferTime = DateTime.UtcNow;
                                unit.LocationId = subTransfer.DestinationLocationId;
                                _context.Units.Update(unit);
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    return (true, null);
                }

                return (false, "Transfer State is not Prepared");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return (false, ex.Message);
            }
        }

        /*
        /// <summary>
        /// Gets all entries from DB for jQuery Datatables
        /// </summary>
        /// <param name="request">DataTableRequest with table search and sort options</param>
        /// <param name="getDeleted">Looks through soft deleted entries</param>
        /// <returns>DataTableDbResult</returns>
        public async Task<DataTableDbResult<MainTransferModel>> GetSubTransfersAsync(DataTableRequest request, bool getDeleted = false)
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