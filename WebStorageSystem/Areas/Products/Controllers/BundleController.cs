using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class BundleController : Controller
    {
        private readonly BundleService _bundleService;
        private readonly UnitService _unitService;
        private readonly IMapper _mapper;

        public BundleController(BundleService bundleService, UnitService unitService, IMapper mapper)
        {
            _bundleService = bundleService;
            _unitService = unitService;
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
        public async Task<IActionResult> Create()
        {
            await CreateUnitDropdownList();
            return View();
        }

        // POST: Products/Bundle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,InventoryNumber,BundledUnitsIds,IsDeleted")] BundleModel bundleModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList();
                return View(bundleModel);
            }

            var units = await _unitService.GetUnitsAsync(bundleModel.BundledUnitsIds, getDeleted);
            var bundle = _mapper.Map<Bundle>(bundleModel);
            await _bundleService.AddBundleAsync(bundle, units);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Bundle/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var bundle = await _bundleService.GetBundleAsync((int)id, getDeleted);
            if (bundle == null) return NotFound();
            var bundleModel = _mapper.Map<BundleModel>(bundle);
            await CreateUnitDropdownList(bundleModel.BundledUnits, true);
            return View(bundleModel);
        }

        // POST: Products/Bundle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,InventoryNumber,BundledUnitsIds,Id,CreatedDate,IsDeleted,RowVersion")] BundleModel bundleModel)
        {
            if (id != bundleModel.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(bundleModel.BundledUnits);
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
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            await _bundleService.RestoreBundleAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _bundleService.GetBundlesAsync(request);
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
    }
}
