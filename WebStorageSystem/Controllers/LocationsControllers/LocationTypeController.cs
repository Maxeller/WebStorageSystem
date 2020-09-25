using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Data;
using WebStorageSystem.Data.Entities.Locations;
using WebStorageSystem.Data.Services.Locations;
using WebStorageSystem.Models.LocationModels;

namespace WebStorageSystem.Controllers.LocationsControllers
{
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

            try
            {
                await _service.EditLocationTypeAsync(locationType);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _service.GetLocationTypeAsync(locationType.Id) == null) return NotFound();
                throw; // TODO: Handle exception
            }
        }

        // POST: LocationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _service.DeleteLocationTypeAsync((int) id);
            return RedirectToAction(nameof(Index));
        }

        // POST: LocationType/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.LocationTypeExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreLocationType((int) id);
            return RedirectToAction(nameof(Index));
        }
    }
}
