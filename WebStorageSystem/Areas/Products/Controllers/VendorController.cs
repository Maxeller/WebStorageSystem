using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class VendorController : Controller
    {
        private readonly VendorService _service;
        private readonly IMapper _mapper;

        public VendorController(VendorService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: VendorModels
        public async Task<IActionResult> Index([FromQuery] bool getDeleted)
        {
            if(getDeleted) HttpContext.Session.SetInt32("GetDeleted", 1);
            var vendors = await _service.GetVendorsAsync(getDeleted);
            var vendorModels = _mapper.Map<IEnumerable<VendorModel>>(vendors);
            return View(vendorModels);
        }

        // GET: VendorModels/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var vendor = await _service.GetVendorAsync((int) id, getDeleted);
            if (vendor == null) return NotFound();
            var vendorModel = _mapper.Map<VendorModel>(vendor);

            return View(vendorModel);
        }

        // GET: VendorModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VendorModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Address,Phone,Email,IsDeleted")] VendorModel vendorModel)
        {
            if (!ModelState.IsValid) return View(vendorModel);

            var vendor = _mapper.Map<Vendor>(vendorModel);
            await _service.AddVendorAsync(vendor);
            return RedirectToAction(nameof(Index));
        }

        // GET: VendorModels/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var vendor = await _service.GetVendorAsync((int) id, getDeleted);
            if (vendor == null) return NotFound();
            var vendorModel = _mapper.Map<VendorModel>(vendor);

            return View(vendorModel);
        }

        // POST: VendorModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Address,Phone,Email,Id,CreatedDate,IsDeleted,RowVersion")] VendorModel vendorModel)
        {
            if (id != vendorModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(vendorModel);

            var vendor = _mapper.Map<Vendor>(vendorModel);

            var (success, errorMessage) = await _service.EditVendorAsync(vendor);
            if(success) return RedirectToAction(nameof(Index));
            if (await _service.GetVendorAsync(vendor.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(vendorModel);
        }

        // POST: VendorModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _service.DeleteVendorAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        // POST: VendorModels/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.VendorExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreVendorAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

    }
}
