using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class UnitController : Controller
    {
        private readonly UnitService _service;
        private readonly ProductService _pService;
        private readonly LocationService _lService;
        private readonly VendorService _vService;
        private readonly BundleService _bService;
        private readonly IMapper _mapper;

        public UnitController(UnitService service, ProductService pService, LocationService lService, VendorService vService, BundleService bService, IMapper mapper)
        {
            _service = service;
            _pService = pService;
            _lService = lService;
            _vService = vService;
            _bService = bService;
            _mapper = mapper;
        }

        // GET: Products/Unit
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new UnitModel());
        }

        // GET: Products/Unit/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var unit = await _service.GetUnitAsync((int) id, getDeleted);
            if (unit == null) return NotFound();
            var unitModel = _mapper.Map<UnitModel>(unit);
            return View(unitModel);
        }

        // GET: Products/Unit/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted)
        {
            await CreateDropdownLists(getDeleted);
            return View();
        }

        // POST: Products/Unit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SerialNumber,ProductId,LocationId,VendorId,PartOfBundleId,IsDeleted")] UnitModel unitModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }

            var product = await _pService.GetProductAsync(unitModel.ProductId, getDeleted);
            var location = await _lService.GetLocationAsync(unitModel.LocationId, getDeleted);
            if (product == null || location == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }
            var unit = _mapper.Map<Unit>(unitModel);
            unit.Product = product;
            unit.Location = location;
            if(unitModel.VendorId != null) unit.Vendor = await _vService.GetVendorAsync((int) unitModel.VendorId, getDeleted);
            if(unitModel.PartOfBundleId != null) unit.PartOfBundle = await _bService.GetBundleAsync((int) unitModel.PartOfBundleId, getDeleted);
            await _service.AddUnitAsync(unit);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Unit/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var unit = await _service.GetUnitAsync((int) id, getDeleted);
            if (unit == null) return NotFound();
            var unitModel = _mapper.Map<UnitModel>(unit);

            await CreateDropdownLists(getDeleted, unitModel.Product, unitModel.Location, unitModel.Vendor);
            return View(unitModel);
        }

        // POST: Products/Unit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SerialNumber,ProductId,LocationId,VendorId,PartOfBundleId,Id,CreatedDate,IsDeleted,RowVersion")] UnitModel unitModel, [FromQuery] bool getDeleted)
        {
            if (id != unitModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(unitModel);

            var product = await _pService.GetProductAsync(unitModel.ProductId, getDeleted);
            var location = await _lService.GetLocationAsync(unitModel.LocationId, getDeleted);
            if (product == null || location == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }
            var unit = _mapper.Map<Unit>(unitModel);
            unit.Product = product;
            unit.Location = location;
            unit.Vendor = unitModel.VendorId != null
                ? await _vService.GetVendorAsync((int) unitModel.VendorId, getDeleted)
                : null;

            unit.PartOfBundle = unitModel.PartOfBundleId != null
                ? await _bService.GetBundleAsync((int) unitModel.PartOfBundleId, getDeleted)
                : null;

            var (success, errorMessage) = await _service.EditUnitAsync(unit);
            if(success) return RedirectToAction(nameof(Index));

            if (await _service.GetUnitAsync(unit.Id) == null) return NotFound();
            await CreateDropdownLists(getDeleted);
            TempData["Error"] = errorMessage;
            return View(unitModel);
        }

        // POST: Products/Unit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _service.DeleteUnitAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Unit/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.UnitExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreUnitAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonSerializer.Serialize(param));
                var results = await _service.GetUnitsAsync(param);
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

                return new JsonResult(new JqueryDataTablesResult<UnitModel> {
                    Draw = param.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
                });
            } catch(Exception e)
            {
                Console.Write(e.Message);
                return new JsonResult(new { error = "Internal Server Error" });
            }
        }

        private async Task CreateDropdownLists(bool getDeleted = false, object selectedProduct = null, object selectedLocation = null, object selectedVendor = null, object selectedBundle = null)
        {
            await CreateProductDropdownList(getDeleted, selectedProduct);
            await CreateLocationDropdownList(getDeleted, selectedLocation);
            await CreateVendorDropdownList(getDeleted, selectedVendor);
            await CreateBundleDropdownList(getDeleted, selectedBundle);
        }

        private async Task CreateProductDropdownList(bool getDeleted, object selectedProduct)
        {
            var products = await _pService.GetProductsAsync(getDeleted);
            var pModels = _mapper.Map<IEnumerable<ProductModel>>(products);
            ViewBag.Products = new SelectList(pModels, "Id", "Name", selectedProduct);
        }

        private async Task CreateLocationDropdownList(bool getDeleted, object selectedLocation)
        {
            var locations = await _lService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<IEnumerable<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedLocation);
        }

        private async Task CreateVendorDropdownList(bool getDeleted, object selectedVendor)
        {
            var vendors = await _vService.GetVendorsAsync(getDeleted);
            var vModels = _mapper.Map<IEnumerable<VendorModel>>(vendors);
            ViewBag.Vendors = new SelectList(vModels, "Id", "Name", selectedVendor);
        }

        private async Task CreateBundleDropdownList(bool getDeleted, object selectedBundle)
        {
            var bundles = await _bService.GetBundlesAsync(getDeleted);
            var bModels = _mapper.Map<IEnumerable<BundleModel>>(bundles);
            ViewBag.Bundles = new SelectList(bModels, "Id", "Name", selectedBundle);
        }
    }
}
