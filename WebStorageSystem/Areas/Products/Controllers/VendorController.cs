using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
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
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new VendorModel());
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

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonSerializer.Serialize(param));
                var results = await _service.GetVendorsAsync(param);
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

                return new JsonResult(new JqueryDataTablesResult<VendorModel> {
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

    }
}
