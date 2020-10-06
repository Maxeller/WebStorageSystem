using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class ManufacturerController : Controller
    {
        private readonly ManufacturerService _service;
        private readonly IMapper _mapper;

        public ManufacturerController(ManufacturerService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: Manufacturer
        public async Task<IActionResult> Index([FromQuery] bool getDeleted)
        {
            var manufacturers = await _service.GetManufacturersAsync(getDeleted);
            var manufacturerModels = _mapper.Map<IEnumerable<ManufacturerModel>>(manufacturers);
            return View(manufacturerModels);
        }

        // GET: Manufacturer/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var manufacturer = await _service.GetManufacturerAsync((int) id, getDeleted);
            var manufacturerModel = _mapper.Map<ManufacturerModel>(manufacturer);
            if (manufacturerModel == null) return NotFound();

            return View(manufacturerModel);
        }

        // GET: Manufacturer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Manufacturer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] ManufacturerModel manufacturerModel)
        {
            if (!ModelState.IsValid) return View(manufacturerModel);

            var manufacturer = _mapper.Map<Manufacturer>(manufacturerModel);
            await _service.AddManufacturerAsync(manufacturer);
            return RedirectToAction(nameof(Index));
        }

        // GET: Manufacturer/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var manufacturer = await _service.GetManufacturerAsync((int) id, getDeleted);
            var manufacturerModel = _mapper.Map<ManufacturerModel>(manufacturer);
            if (manufacturerModel == null) return NotFound();

            return View(manufacturerModel);
        }

        // POST: Manufacturer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedDate,IsDeleted,RowVersion")] ManufacturerModel manufacturerModel)
        {
            if (id != manufacturerModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(manufacturerModel);

            var manufacturer = _mapper.Map<Manufacturer>(manufacturerModel);

            var (success, errorMessage) = await _service.EditManufacturerAsync(manufacturer);
            if(success) return RedirectToAction(nameof(Index));
            if (await _service.GetManufacturerAsync(manufacturer.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(manufacturerModel);
        }

        // POST: Manufacturer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _service.DeleteManufacturerAsync((int) id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: Manufacturer/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.ManufacturerExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreManufacturerAsync((int) id);
            return RedirectToAction(nameof(Index));
        }
    }
}
