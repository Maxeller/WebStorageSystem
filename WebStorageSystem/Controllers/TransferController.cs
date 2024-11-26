using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Data.Services;
using WebStorageSystem.Models.DataTables;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Controllers
{
    [Authorize(Roles = "User, Warehouse, Admin")]
    public class TransferController : Controller
    {
        private readonly TransferService _transferService;
        private readonly LocationService _locationService;
        private readonly IMapper _mapper;

        public TransferController(TransferService transferService, LocationService locationService, IMapper mapper)
        {
            _transferService = transferService;
            _locationService = locationService;
            _mapper = mapper;
        }

        // GET: Transfer/Index
        public IActionResult Index()
        {
            return View(new SubTransferModel());
        }

        // GET: Transfer/Create
        public async Task<IActionResult> Create()
        {
            await CreateLocationDropdownList();
            return View();
        }

        // POST: Transfer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferNumber,DestinationLocationId")] MainTransferModel mainTransferModel, TransferState state, string selectedRowsJson)
        {
            var selectedRows = JsonSerializer.Deserialize<List<UnitBundleViewModel>>(selectedRowsJson); // Deserialization in method doesnt work (for some reason)

            if (!ModelState.IsValid && selectedRows.Count != 0)
            {
                await CreateLocationDropdownList();
                return View();
            }

            mainTransferModel.State = state;
            var mainTransfer = _mapper.Map<MainTransfer>(mainTransferModel);

            await _transferService.AddTransferAsync(mainTransfer, selectedRows);
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Transfer/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var mainTransfer = await _transferService.GetTransferAsync(id);
            if (mainTransfer == null) return NotFound();
            var model = _mapper.Map<MainTransferModel>(mainTransfer);
            return View(model);
        }

        public async Task<IActionResult> Transfer(int id)
        {
            await _transferService.Transfer(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/LoadTable
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _transferService.GetSubTransfersAsync(request);
                foreach (var item in results.Data)
                {
                    RouteValueDictionary dict = new RouteValueDictionary { { "id", item.MainTransferId } };
                    item.Action = new Dictionary<string, string>();
                    if (item.MainTransfer.State == TransferState.Prepared)
                    {
                        item.Action.Add("Transfer", Url.Action(nameof(Transfer), dict));
                    }
                    item.Action.Add("Details", Url.Action(nameof(Details), dict));
                }

                return new JsonResult(new DataTableResponse<SubTransferModel>
                {
                    Draw = request.Draw,
                    Data = results.Data,
                    RecordsFiltered = results.RecordsTotal,
                    RecordsTotal = results.RecordsTotal
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        // POST: Transfer/LoadUnitBundleView
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadUnitBundleView(DataTableRequest request)
        {
            try
            {
                var results = await _transferService.GetUnitBundleViewAsync(request);

                return new JsonResult(new DataTableResponse<UnitBundleViewModel>
                {
                    Draw = request.Draw,
                    Data = results.Data,
                    RecordsFiltered = results.RecordsTotal,
                    RecordsTotal = results.RecordsTotal
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        // POST: Transfer/LoadTableDetails
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTableDetails(DataTableRequest request)
        {
            try
            {
                var results = await _transferService.GetSubTransfersForDetailAsync(request);

                return new JsonResult(new DataTableResponse<SubTransferModel>
                {
                    Draw = request.Draw,
                    Data = results.Data,
                    RecordsFiltered = results.RecordsTotal,
                    RecordsTotal = results.RecordsTotal
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        private async Task CreateLocationDropdownList(bool getDeleted = false, object selectedUnits = null)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<ICollection<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "NameType", selectedUnits);
        }
    }
}