using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Areas.Locations.Controllers
{
    [Area("Locations")]
    public class LocationTypeController : Controller
    {
        private readonly LocationTypeService _service;
        private readonly IMapper _mapper;

        public LocationTypeController(LocationTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: LocationType
        public async Task<IActionResult> Index([FromQuery] bool getDeleted = false)
        {
            var locationTypes = await _service.GetLocationTypesAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            return View(models);
        }

        // GET: LocationType/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var locationType = await _service.GetLocationTypeAsync((int) id, getDeleted);
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);
            if (locationTypeModel == null) return NotFound();

            return View(locationTypeModel);
        }

        // GET: LocationType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsDeleted")] LocationTypeModel locationTypeModel)
        {
            if (!ModelState.IsValid) return View(locationTypeModel);

            var locationType = _mapper.Map<LocationType>(locationTypeModel);
            await _service.AddLocationTypeAsync(locationType);
            return RedirectToAction(nameof(Index));
        }

        // GET: LocationType/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var locationType = await _service.GetLocationTypeAsync((int) id, getDeleted);
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);
            if (locationTypeModel == null) return NotFound();

            return View(locationTypeModel);
        }

        // POST: LocationType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedDate,IsDeleted,RowVersion")] LocationTypeModel locationTypeModel)
        {
            if (id != locationTypeModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(locationTypeModel);

            var locationType = _mapper.Map<LocationType>(locationTypeModel);

            var (success, errorMessage) = await _service.EditLocationTypeAsync(locationType);
            if(success) return RedirectToAction(nameof(Index));
            if (await _service.GetLocationTypeAsync(locationType.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(locationTypeModel);
        }

        // POST: LocationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var affectedRows = await _service.DeleteLocationTypeAsync((int) id);
            if (affectedRows == -1) TempData["Error"] = "Location Type cannot be deleted.<br/>It's used as type in existing Location.";
            return RedirectToAction(nameof(Index));
        }

        // POST: LocationType/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.LocationTypeExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreLocationTypeAsync((int) id);
            return RedirectToAction(nameof(Index));
        }
    }
}
