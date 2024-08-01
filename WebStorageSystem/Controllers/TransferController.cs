using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Services;
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
        public async Task<IActionResult> Create([Bind("TransferNumber,OriginLocationId,DestinationLocationId,UnitsIds,IsDeleted")] MainTransferModel mainTransferModel, TransferState state)
        {
            if (!ModelState.IsValid)
            {
                await CreateLocationDropdownList();
                return View();
            }
            /*
            var transfer = _mapper.Map<Transfer>(transferModel);
            var units = new List<Unit>(transferModel.UnitsIds.ToArray().Length);
            foreach (var unitId in transferModel.UnitsIds)
            {
                units.Add(await _unitService.GetUnitAsync(unitId));
            }

            transfer.Units = units;
            transfer.State = state;
            await _transferService.AddTransferAsync(transfer);
            */
            return RedirectToAction(nameof(Index));
            
        }

        // GET: Transfer/Details/5
        public IActionResult Details()
        {
            //return View();
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/LoadTable
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _transferService.GetTransfersAsync(request);
                foreach (var item in results.Data)
                {
                    item.Action = new Dictionary<string, string>
                    {
                        {"Details", Url.Action(nameof(Details), new {item.MainTransferId})}
                    };
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

        // GET: Transfer/UnitLoc
        public async Task<Select2AjaxResult> UnitLoc(int loc, string sn)
        {
            var result = new Select2AjaxResult();

            var units = await _unitService.GetUnitsAsync();
            var unitsAtLoc = units
                .ToList()
                .FindAll(unit => unit.Location.Id == loc &&
                                 unit.InventoryNumber.Contains(sn ?? "", StringComparison.OrdinalIgnoreCase))
                .AsParallel()
                .ToList();
            foreach (var unit in unitsAtLoc) // TODO: Change to Parallel.ForEach ?
            {
                result.Results.Add(new Select2AjaxPartResult(unit.Id, unit.InventoryNumber));
            }

            return result;
        }
        
        private async Task CreateLocationDropdownList(bool getDeleted = false, object selectedUnits = null)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<ICollection<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedUnits);
        }
    }
}