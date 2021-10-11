using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Data.Services;
using WebStorageSystem.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Controllers
{
    //[Authorize]
    public class TransferController : Controller
    {
        private readonly TransferService _service;
        private readonly LocationService _lService;
        private readonly UnitService _uService;
        private readonly IMapper _mapper;

        public TransferController(TransferService service, LocationService lService, UnitService uService,
            IMapper mapper)
        {
            _service = service;
            _lService = lService;
            _uService = uService;
            _mapper = mapper;
        }

        // GET: Transfer/Index
        public IActionResult Index()
        {
            return View(new TransferModel());
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
        public async Task<IActionResult> Create([Bind("TransferNumber,OriginLocationId,DestinationLocationId,UnitsIds,IsDeleted")] TransferModel transferModel, TransferState state)
        {
            if (!ModelState.IsValid)
            {
                await CreateLocationDropdownList();
                return View();
            }

            var transfer = _mapper.Map<Transfer>(transferModel);
            var units = new List<Unit>(transferModel.UnitsIds.ToArray().Length);
            foreach (var unitId in transferModel.UnitsIds)
            {
                units.Add(await _uService.GetUnitAsync(unitId));
            }

            transfer.Units = units;
            transfer.State = state;
            await _service.AddTransferAsync(transfer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Transfer/Details/5
        public IActionResult Details()
        {
            //return View();
            return RedirectToAction(nameof(Index));
        }

        // GET: Transfer/Edit/5
        public IActionResult Edit()
        {
            //return View();
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            return RedirectToAction(nameof(Index));
        }

        // POST: Transfer/LoadTable
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _service.GetTransfersAsync(request);
                foreach (var item in results.Data)
                {
                    item.Action = new Dictionary<string, string>
                    {
                        {"Edit", Url.Action(nameof(Edit), new {item.Id})},
                        {"Details", Url.Action(nameof(Details), new {item.Id})},
                        {"Delete", Url.Action(nameof(Delete), new {item.Id})},
                        {"Restore", Url.Action(nameof(Restore), new {item.Id})}
                    };
                }

                return new JsonResult(new DataTableResponse<TransferModel>
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

            var units = await _uService.GetUnitsAsync();
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
            var locations = await _lService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<ICollection<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedUnits);
        }
    }
}