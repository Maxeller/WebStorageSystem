using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize(Roles = "Admin, Warehouse, User")]
    public class ManufacturerController : Controller
    {
        private readonly ManufacturerService _manufacturerService;
        private readonly IMapper _mapper;

        public ManufacturerController(ManufacturerService manufacturerService, IMapper mapper)
        {
            _manufacturerService = manufacturerService;
            _mapper = mapper;
        }

        // GET: Manufacturer
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new ManufacturerModel());
        }

        // GET: Manufacturer/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var manufacturer = await _manufacturerService.GetManufacturerAsync((int)id, getDeleted);
            if (manufacturer == null) return NotFound();
            var manufacturerModel = _mapper.Map<ManufacturerModel>(manufacturer);

            return View(manufacturerModel);
        }

        // GET: Manufacturer/Create
        [Authorize(Roles = "Admin, Warehouse")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Create([Bind("Name,Description")] ManufacturerModel manufacturerModel)
        {
            if (!ModelState.IsValid) return View(manufacturerModel);

            var manufacturer = _mapper.Map<Manufacturer>(manufacturerModel);
            await _manufacturerService.AddManufacturerAsync(manufacturer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Manufacturer/Edit/5
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var manufacturer = await _manufacturerService.GetManufacturerAsync((int)id, getDeleted);
            if (manufacturer == null) return NotFound();
            var manufacturerModel = _mapper.Map<ManufacturerModel>(manufacturer);

            return View(manufacturerModel);
        }

        // POST: Manufacturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedDate,IsDeleted,RowVersion")] ManufacturerModel manufacturerModel)
        {
            if (id != manufacturerModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(manufacturerModel);

            var manufacturer = _mapper.Map<Manufacturer>(manufacturerModel);

            var (success, errorMessage) = await _manufacturerService.EditManufacturerAsync(manufacturer);
            if (success) return RedirectToAction(nameof(Index));
            if (await _manufacturerService.GetManufacturerAsync(manufacturer.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(manufacturerModel);
        }

        // POST: Manufacturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _manufacturerService.DeleteManufacturerAsync((int)id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: Manufacturer/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _manufacturerService.ManufacturerExistsAsync((int)id, true))) return NotFound();
            await _manufacturerService.RestoreManufacturerAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _manufacturerService.GetManufacturersAsync(request);
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

                return new JsonResult(new DataTableResponse<ManufacturerModel>
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
