using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class UnitController : Controller
    {
        private readonly UnitService _unitService;
        private readonly ProductService _productService;
        private readonly LocationService _locationService;
        private readonly VendorService _vendorService;
        private readonly BundleService _bundleService;
        private readonly IMapper _mapper;

        public UnitController(UnitService unitService, ProductService productService, LocationService locationService, VendorService vendorService, BundleService bundleService, IMapper mapper)
        {
            _unitService = unitService;
            _productService = productService;
            _locationService = locationService;
            _vendorService = vendorService;
            _bundleService = bundleService;
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

            var unit = await _unitService.GetUnitAsync((int)id, getDeleted);
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
        public async Task<IActionResult> Create([Bind("InventoryNumber,SerialNumber,ProductId,LocationId,DefaultLocationId,VendorId,PartOfBundleId,ShelfNumber,Notes,IsDeleted")] UnitModel unitModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }

            var product = await _productService.GetProductAsync(unitModel.ProductId, getDeleted);
            var location = await _locationService.GetLocationAsync(unitModel.LocationId, getDeleted);
            var defaultLocation = await _locationService.GetLocationAsync(unitModel.DefaultLocationId, getDeleted);
            if (product == null || location == null || defaultLocation == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }
            var unit = _mapper.Map<Unit>(unitModel);
            unit.Product = product;
            unit.Location = location;
            if (unitModel.VendorId != null) unit.Vendor = await _vendorService.GetVendorAsync((int)unitModel.VendorId, getDeleted);
            if (unitModel.PartOfBundleId != null) unit.PartOfBundle = await _bundleService.GetBundleAsync((int)unitModel.PartOfBundleId, getDeleted);
            await _unitService.AddUnitAsync(unit);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Unit/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var unit = await _unitService.GetUnitAsync((int)id, getDeleted);
            if (unit == null) return NotFound();
            var unitModel = _mapper.Map<UnitModel>(unit);

            await CreateDropdownLists(getDeleted, unitModel.Product, unitModel.Location, unitModel.DefaultLocation, unitModel.Vendor);
            return View(unitModel);
        }

        // POST: Products/Unit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryNumber,SerialNumber,ProductId,LocationId,DefaultLocationId,VendorId,PartOfBundleId,ShelfNumber,Notes,LastTransferTime,LastCheckTime,Id,CreatedDate,IsDeleted,RowVersion")] UnitModel unitModel, [FromQuery] bool getDeleted)
        {
            if (id != unitModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(unitModel);

            var product = await _productService.GetProductAsync(unitModel.ProductId, getDeleted);
            var location = await _locationService.GetLocationAsync(unitModel.LocationId, getDeleted);
            var defaultLocation = await _locationService.GetLocationAsync(unitModel.DefaultLocationId, getDeleted);
            if (product == null || location == null || defaultLocation == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(unitModel);
            }
            var unit = _mapper.Map<Unit>(unitModel);
            unit.Product = product;
            unit.Location = location;
            unit.Vendor = unitModel.VendorId != null
                ? await _vendorService.GetVendorAsync((int)unitModel.VendorId, getDeleted)
                : null;

            unit.PartOfBundle = unitModel.PartOfBundleId != null
                ? await _bundleService.GetBundleAsync((int)unitModel.PartOfBundleId, getDeleted)
                : null;

            var (success, errorMessage) = await _unitService.EditUnitAsync(unit);
            if (success) return RedirectToAction(nameof(Index));

            if (await _unitService.GetUnitAsync(unit.Id) == null) return NotFound();
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
            await _unitService.DeleteUnitAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Unit/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _unitService.UnitExistsAsync((int)id, true))) return NotFound();
            await _unitService.RestoreUnitAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _unitService.GetUnitsAsync(request);
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

                return new JsonResult(new DataTableResponse<UnitModel>
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

        private async Task CreateDropdownLists(bool getDeleted = false, object selectedProduct = null, object selectedLocation = null, object selectedDefaultLocation = null, object selectedVendor = null, object selectedBundle = null)
        {
            await CreateProductDropdownList(getDeleted, selectedProduct);
            await CreateLocationDropdownList(getDeleted, selectedLocation);
            await CreateDefaultLocationDropdownList(getDeleted, selectedDefaultLocation);
            await CreateVendorDropdownList(getDeleted, selectedVendor);
            await CreateBundleDropdownList(getDeleted, selectedBundle);
        }

        private async Task CreateProductDropdownList(bool getDeleted, object selectedProduct)
        {
            var products = await _productService.GetProductsAsync(getDeleted);
            var pModels = _mapper.Map<IEnumerable<ProductModel>>(products);
            ViewBag.Products = new SelectList(pModels, "Id", "Name", selectedProduct);
        }

        private async Task CreateLocationDropdownList(bool getDeleted, object selectedLocation)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<IEnumerable<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedLocation);
        }

        private async Task CreateDefaultLocationDropdownList(bool getDeleted, object selectedLocation)
        {
            var locations = await _locationService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<IEnumerable<LocationModel>>(locations);
            ViewBag.DefaultLocations = new SelectList(lModels, "Id", "Name", selectedLocation);
        }

        private async Task CreateVendorDropdownList(bool getDeleted, object selectedVendor)
        {
            var vendors = await _vendorService.GetVendorsAsync(getDeleted);
            var vModels = _mapper.Map<IEnumerable<VendorModel>>(vendors);
            ViewBag.Vendors = new SelectList(vModels, "Id", "Name", selectedVendor);
        }

        private async Task CreateBundleDropdownList(bool getDeleted, object selectedBundle)
        {
            var bundles = await _bundleService.GetBundlesAsync(getDeleted);
            var bModels = _mapper.Map<IEnumerable<BundleModel>>(bundles);
            ViewBag.Bundles = new SelectList(bModels, "Id", "Name", selectedBundle);
        }
    }
}
