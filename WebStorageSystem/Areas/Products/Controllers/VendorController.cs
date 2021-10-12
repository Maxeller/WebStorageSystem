using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class VendorController : Controller
    {
        private readonly VendorService _vendorService;
        private readonly IMapper _mapper;

        public VendorController(VendorService vendorService, IMapper mapper)
        {
            _vendorService = vendorService;
            _mapper = mapper;
        }

        // GET: VendorModels
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new VendorModel());
        }

        // GET: VendorModels/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var vendor = await _vendorService.GetVendorAsync((int)id, getDeleted);
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
        public async Task<IActionResult> Create([Bind("Name,Address,Phone,Email,Website,IsDeleted")] VendorModel vendorModel)
        {
            if (!ModelState.IsValid) return View(vendorModel);

            var vendor = _mapper.Map<Vendor>(vendorModel);
            await _vendorService.AddVendorAsync(vendor);
            return RedirectToAction(nameof(Index));
        }

        // GET: VendorModels/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var vendor = await _vendorService.GetVendorAsync((int)id, getDeleted);
            if (vendor == null) return NotFound();
            var vendorModel = _mapper.Map<VendorModel>(vendor);

            return View(vendorModel);
        }

        // POST: VendorModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Address,Phone,Email,Website,Id,CreatedDate,IsDeleted,RowVersion")] VendorModel vendorModel)
        {
            if (id != vendorModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(vendorModel);

            var vendor = _mapper.Map<Vendor>(vendorModel);

            var (success, errorMessage) = await _vendorService.EditVendorAsync(vendor);
            if (success) return RedirectToAction(nameof(Index));
            if (await _vendorService.GetVendorAsync(vendor.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(vendorModel);
        }

        // POST: VendorModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _vendorService.DeleteVendorAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: VendorModels/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _vendorService.VendorExistsAsync((int)id, true))) return NotFound();
            await _vendorService.RestoreVendorAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _vendorService.GetVendorsAsync(request);
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

                return new JsonResult(new DataTableResponse<VendorModel>
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
    }
}