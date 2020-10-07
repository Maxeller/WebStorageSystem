using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public UnitController(UnitService service, ProductService pService, LocationService lService, VendorService vService, IMapper mapper)
        {
            _service = service;
            _pService = pService;
            _lService = lService;
            _vService = vService;
            _mapper = mapper;
        }

        // GET: Products/Unit
        public async Task<IActionResult> Index([FromQuery] bool getDeleted)
        {
            var units = await _service.GetUnitsAsync(getDeleted);
            var unitModels = _mapper.Map<IEnumerable<UnitModel>>(units);
            return View(unitModels);
        }

        // GET: Products/Unit/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var unit = await _service.GetUnitAsync((int) id, getDeleted);
            var unitModel = _mapper.Map<UnitModel>(unit);
            if (unitModel == null) return NotFound();
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
        public async Task<IActionResult> Create([Bind("SerialNumber,ProductId,LocationId,VendorId,IsDeleted")] UnitModel unitModel, [FromQuery] bool getDeleted)
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
            await _service.AddUnitAsync(unit);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Unit/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var unit = await _service.GetUnitAsync((int) id, getDeleted);
            var unitModel = _mapper.Map<UnitModel>(unit);
            if (unitModel == null) return NotFound();

            await CreateDropdownLists(getDeleted, unitModel.Product, unitModel.Location, unitModel.Vendor);
            return View(unitModel);
        }

        // POST: Products/Unit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SerialNumber,ProductId,LocationId,VendorId,Id,CreatedDate,IsDeleted")] UnitModel unitModel, [FromQuery] bool getDeleted)
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
            if(unitModel.VendorId != null) unit.Vendor = await _vService.GetVendorAsync((int) unitModel.VendorId, getDeleted);

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

        private async Task CreateDropdownLists(bool getDeleted = false, object selectedProduct = null, object selectedLocation = null, object selectedVendor = null)
        {
            await CreateProductDropdownList(getDeleted, selectedProduct);
            await CreateLocationDropdownList(getDeleted, selectedLocation);
            await CreateVendorDropdownList(getDeleted, selectedVendor);
        }

        private async Task CreateProductDropdownList(bool getDeleted = false, object selectedProduct = null)
        {
            var products = await _pService.GetProductsAsync(getDeleted);
            var pModels = _mapper.Map<IEnumerable<ProductModel>>(products);
            ViewBag.Products = new SelectList(pModels, "Id", "Name", selectedProduct);
        }

        private async Task CreateLocationDropdownList(bool getDeleted = false, object selectedLocation = null)
        {
            var locations = await _lService.GetLocationsAsync(getDeleted);
            var lModels = _mapper.Map<IEnumerable<LocationModel>>(locations);
            ViewBag.Locations = new SelectList(lModels, "Id", "Name", selectedLocation);
        }

        private async Task CreateVendorDropdownList(bool getDeleted = false, object selectedVendor = null)
        {
            var vendors = await _vService.GetVendorsAsync(getDeleted);
            var vModels = _mapper.Map<IEnumerable<VendorModel>>(vendors);
            ViewBag.Vendors = new SelectList(vModels, "Id", "Name", selectedVendor);
        }
    }
}
