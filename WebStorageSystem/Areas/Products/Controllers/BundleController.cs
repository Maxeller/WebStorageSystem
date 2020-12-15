using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class BundleController : Controller
    {
        private readonly BundleService _service;
        private readonly UnitService _uService;
        private readonly IMapper _mapper;

        public BundleController(BundleService service, UnitService uService, IMapper mapper)
        {
            _service = service;
            _uService = uService;
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
            var bundle = await _service.GetBundleAsync((int)id, getDeleted);
            if (bundle == null) return NotFound();
            var bundleModel = _mapper.Map<BundleModel>(bundle);
            return View(bundleModel);
        }

        // GET: Products/Bundle/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted)
        {
            await CreateUnitDropdownList(getDeleted);
            return View();
        }

        // POST: Products/Bundle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SerialNumber,BundledUnitsIds,IsDeleted")] BundleModel bundleModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(getDeleted);
                return View(bundleModel);
            }

            var units = await _uService.GetUnitAsync(bundleModel.BundledUnitsIds, getDeleted);
            var bundle = _mapper.Map<Bundle>(bundleModel);
            await _service.AddBundleAsync(bundle, units);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Bundle/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();
            var bundle = await _service.GetBundleAsync((int)id, getDeleted);
            if (bundle == null) return NotFound();
            var bundleModel = _mapper.Map<BundleModel>(bundle);
            await CreateUnitDropdownList(getDeleted, bundleModel.BundledUnits);
            return View(bundleModel);
        }

        // POST: Products/Bundle/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,SerialNumber,BundledUnitsIds,Id,CreatedDate,IsDeleted,RowVersion")] BundleModel bundleModel, [FromQuery] bool getDeleted)
        {
            if (id != bundleModel.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(getDeleted, bundleModel.BundledUnits);
                return View(bundleModel);
            }

            var units = await _uService.GetUnitAsync(bundleModel.BundledUnitsIds, getDeleted);
            var bundle = _mapper.Map<Bundle>(bundleModel);
            var (success, errorMessage) = await _service.EditBundleAsync(bundle, units);
            if (success) return RedirectToAction(nameof(Index));

            if (await _service.GetBundleAsync(bundle.Id) == null) return NotFound();
            await CreateUnitDropdownList(getDeleted);
            TempData["Error"] = errorMessage;
            return View(bundleModel);
        }

        // POST: Products/Bundle/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _service.DeleteBundleAsync((int)id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Bundle/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            await _service.RestoreBundleAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonSerializer.Serialize(param));
                var results = await _service.GetBundlesAsync(param);
                foreach (var item in results.Items)
                {
                    item.Action = new Dictionary<string, string>
                    {
                        {"Edit", Url.Action(nameof(Edit), new {item.Id})},
                        {"Details", Url.Action(nameof(Details), new {item.Id})},
                        {"Delete", Url.Action(nameof(Delete), new {item.Id})},
                        {"Restore", Url.Action(nameof(Restore), new {item.Id})}
                    };
                }

                return new JsonResult(new JqueryDataTablesResult<BundleModel>
                {
                    Draw = param.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
                });
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        private async Task CreateUnitDropdownList(bool getDeleted = false, IEnumerable<UnitModel> selectedValues = null)
        {
            var units = await _uService.GetUnitsAsync(getDeleted);
            var unitModels = _mapper.Map<ICollection<UnitModel>>(units);
            ViewBag.Units = new MultiSelectList(unitModels, "Id", "SerialNumberProduct", (selectedValues ?? Array.Empty<UnitModel>()).Select(s => s.Id).ToList());
        }
    }
}
