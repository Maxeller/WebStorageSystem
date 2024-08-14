using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Data.Services;
using WebStorageSystem.Models;
using WebStorageSystem.Models.DataTables;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Controllers
{
    //[Authorize]
    public class TransferController : Controller
    {
        private readonly TransferService _transferService;
        private readonly LocationService _locationService;
        private readonly UnitService _unitService;
        private readonly IMapper _mapper;

        public TransferController(TransferService transferService, LocationService locationService, UnitService unitService,
            IMapper mapper)
        {
            _transferService = transferService;
            _locationService = locationService;
            _unitService = unitService;
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
        public IActionResult Details()
        {
            return View();
        }

        public async Task<IActionResult> Transfer(int id)
        {
            await _transferService.Transfer(id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/LoadTable
        [HttpPost]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _transferService.GetTransfersAsync(request);
                foreach (var item in results.Data)
                {
                    item.Action = new Dictionary<string, string>();
                    if (item.MainTransfer.State == TransferState.Prepared)
                    {
                        item.Action.Add("Transfer", Url.Action(nameof(Transfer), new { item.MainTransferId }));
                    }
                    item.Action.Add("Details", Url.Action(nameof(Details), new { item.MainTransferId }));
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
        
        private async Task CreateLocationDropdownList(bool getDeleted = false, object selectedUnits = null)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<ICollection<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedUnits);
        }
    }
}