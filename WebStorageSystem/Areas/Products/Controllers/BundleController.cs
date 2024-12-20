﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize(Roles = "Admin, Warehouse, User")]
    public class BundleController : Controller
    {
        private readonly BundleService _bundleService;
        private readonly UnitService _unitService;
        private readonly LocationService _locationService;
        private readonly IMapper _mapper;

        public BundleController(BundleService bundleService, UnitService unitService, LocationService locationService, IMapper mapper)
        {
            _bundleService = bundleService;
            _unitService = unitService;
            _locationService = locationService;
            _mapper = mapper;
        }

        // GET: Products/Bundle
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new BundleModel());
        }

        // GET: Products/Bundle/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var bundle = await _bundleService.GetBundleAsync((int)id, getDeleted);
            if (bundle == null) return NotFound();
            var bundleModel = _mapper.Map<BundleModel>(bundle);
            return View(bundleModel);
        }

        // GET: Products/Bundle/Create
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Create()
        {
            await CreateUnitDropdownList();
            await CreateLocationDropdownList();
            return View();
        }

        // POST: Products/Bundle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Create([Bind("Name,InventoryNumber,BundledUnitsIds,LocationId,DefaultLocationId,IsDeleted")] BundleModel bundleModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList();
                await CreateLocationDropdownList();
                return View(bundleModel);
            }

            var units = await _unitService.GetUnitsAsync(bundleModel.BundledUnitsIds, getDeleted);
            var bundle = _mapper.Map<Bundle>(bundleModel);
            await _bundleService.AddBundleAsync(bundle, units);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Bundle/Edit/5
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var bundle = await _bundleService.GetBundleAsync((int)id, getDeleted);
            if (bundle == null) return NotFound();
            var bundleModel = _mapper.Map<BundleModel>(bundle);
            await CreateUnitDropdownList(bundleModel.BundledUnits, true);
            await CreateLocationDropdownList(bundleModel.Location, bundleModel.DefaultLocation);
            return View(bundleModel);
        }

        // POST: Products/Bundle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,InventoryNumber,BundledUnitsIds,LocationId,DefaultLocationId,Id,CreatedDate,IsDeleted,RowVersion")] BundleModel bundleModel, bool getDeleted = false)
        {
            if (id != bundleModel.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(bundleModel.BundledUnits);
                await CreateLocationDropdownList(bundleModel.Location, bundleModel.DefaultLocation);
                return View(bundleModel);
            }

            var units = await _unitService.GetUnitsAsync(bundleModel.BundledUnitsIds);
            var bundle = _mapper.Map<Bundle>(bundleModel);

            var (success, errorMessage) = await _bundleService.EditBundleAsync(bundle, units);
            if (success) return RedirectToAction(nameof(Index));

            if (await _bundleService.GetBundleAsync(bundle.Id) == null) return NotFound();
            await CreateUnitDropdownList(bundleModel.BundledUnits, true);
            TempData["Error"] = errorMessage;
            return View(bundleModel);
        }

        // POST: Products/Bundle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _bundleService.DeleteBundleAsync((int)id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Bundle/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            await _bundleService.RestoreBundleAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _bundleService.GetBundlesAsync(request);
                foreach (var item in results.Data)
                {
                    var routeValues = new RouteValueDictionary { { "id", item.Id }, { "getDeleted", item.IsDeleted } };
                    item.Action = new Dictionary<string, string>
                    {
                        {"Edit", Url.Action(nameof(Edit), routeValues)},
                        {"Details", Url.Action(nameof(Details), routeValues)},
                        {"Delete", Url.Action(nameof(Delete), new {item.Id})},
                        {"Restore", Url.Action(nameof(Restore), new {item.Id})}
                    };
                }

                return new JsonResult(new DataTableResponse<BundleModel>
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

        private async Task CreateUnitDropdownList(IEnumerable<UnitModel> selectedValues = null, bool getUnitsAlreadyInBundle = false)
        {
            var units = getUnitsAlreadyInBundle
                ? await _unitService.GetUnitsAsync()
                : await _unitService.GetUnitsNotInBundleAsync();
            var unitModels = _mapper.Map<ICollection<UnitModel>>(units);
            ViewBag.Units = new MultiSelectList(unitModels, "Id", "InventoryNumberProduct", (selectedValues ?? Array.Empty<UnitModel>()).Select(s => s.Id).ToList());
        }

        private async Task CreateLocationDropdownList(object selectedLocation = null, object selectedDefaultLocation = null, bool getDeleted = false)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<ICollection<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "NameType", selectedLocation);
            ViewBag.DefaultLocations = new SelectList(lModels, "Id", "NameType", selectedDefaultLocation);
        }
    }
}
