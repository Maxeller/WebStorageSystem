using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Areas.Locations.Controllers
{
    [Area("Locations")]
    public class LocationController : Controller
    {
        private readonly LocationService _service;
        private readonly LocationTypeService _ltService;
        private readonly IMapper _mapper;

        public LocationController(LocationService service, LocationTypeService ltService, IMapper mapper)
        {
            _service = service;
            _ltService = ltService;
            _mapper = mapper;
        }

        // GET: Location
        public async Task<IActionResult> Index([FromQuery] bool getDeleted = false)
        {
            var locations = await _service.GetLocationsAsync(getDeleted);
            var models = _mapper.Map<ICollection<LocationModel>>(locations);
            return View(models);
        }

        // GET: Location/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _service.GetLocationAsync((int) id, getDeleted);
            var locationModel = _mapper.Map<LocationModel>(location);
            if (locationModel == null) return NotFound();

            return View(locationModel);
        }

        // GET: Location/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted = false)
        {
            await CreateLocationTypeDropdownList(getDeleted);
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,LocationTypeId,IsDeleted")] LocationModel locationModel, [FromQuery] bool getDeleted = false)
        {
            if (!ModelState.IsValid)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }

            var locationType = await _ltService.GetLocationTypeAsync(locationModel.LocationTypeId, getDeleted);
            if (locationType == null)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }
            var location = _mapper.Map<Location>(locationModel);
            location.LocationType = locationType;
            await _service.AddLocationAsync(location);
            return RedirectToAction(nameof(Index));
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _service.GetLocationAsync((int) id, getDeleted);
            var locationModel = _mapper.Map<LocationModel>(location);
            if (locationModel == null) return NotFound();

            await CreateLocationTypeDropdownList(getDeleted, locationModel.LocationType);

            return View(locationModel);
        }

        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,LocationTypeId,Id,CreatedDate,IsDeleted,RowVersion")] LocationModel locationModel, [FromQuery] bool getDeleted)
        {
            if (id != locationModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(locationModel);

            var locationType = await _ltService.GetLocationTypeAsync(locationModel.LocationTypeId, getDeleted);
            if (locationType == null)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }
            var location = _mapper.Map<Location>(locationModel);
            location.LocationType = locationType;

            var (success, errorMessage) = await _service.EditLocationAsync(location);
            if(success) return RedirectToAction(nameof(Index));
            if (await _service.GetLocationAsync(location.Id) == null) return NotFound();
            await CreateLocationTypeDropdownList(getDeleted);
            TempData["Error"] = errorMessage;
            return View(locationModel);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null) return BadRequest();
            await _service.DeleteLocationAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Location/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.LocationExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreLocationAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        private async Task CreateLocationTypeDropdownList(bool getDeleted = false, object selectedType = null)
        {
            var locationTypes = await _ltService.GetLocationTypesAsync(getDeleted);
            var ltModels = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            ViewBag.LocationTypes = new SelectList(ltModels, "Id", "Name", selectedType);
        }
    }
}
