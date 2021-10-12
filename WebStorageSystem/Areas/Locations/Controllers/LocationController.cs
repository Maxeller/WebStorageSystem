using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Locations.Controllers
{
    [Area("Locations")]
    public class LocationController : Controller
    {
        private readonly LocationService _locationService;
        private readonly LocationTypeService _locationTypeService;
        private readonly IMapper _mapper;

        public LocationController(LocationService locationService, LocationTypeService locationTypeService, IMapper mapper)
        {
            _locationService = locationService;
            _locationTypeService = locationTypeService;
            _mapper = mapper;
        }

        // GET: Locations/Location
        public IActionResult Index([FromQuery] bool getDeleted = false)
        {
            return View(new LocationModel());
        }

        // GET: Locations/Location/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _locationService.GetLocationAsync((int)id, getDeleted);
            if (location == null) return NotFound();
            var locationModel = _mapper.Map<LocationModel>(location);

            return View(locationModel);
        }

        // GET: Locations/Location/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted = false)
        {
            await CreateLocationTypeDropdownList(getDeleted);
            return View();
        }

        // POST: Locations/Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Address,LocationTypeId,IsDeleted")] LocationModel locationModel, [FromQuery] bool getDeleted = false)
        {
            if (!ModelState.IsValid)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }

            var locationType = await _locationTypeService.GetLocationTypeAsync(locationModel.LocationTypeId, getDeleted);
            if (locationType == null)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }
            var location = _mapper.Map<Location>(locationModel);
            location.LocationType = locationType;
            await _locationService.AddLocationAsync(location);
            return RedirectToAction(nameof(Index));
        }

        // GET: Locations/Location/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _locationService.GetLocationAsync((int)id, getDeleted);
            if (location == null) return NotFound();
            var locationModel = _mapper.Map<LocationModel>(location);

            await CreateLocationTypeDropdownList(getDeleted, locationModel.LocationType);

            return View(locationModel);
        }

        // POST: Locations/Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Address,LocationTypeId,Id,CreatedDate,IsDeleted,RowVersion")] LocationModel locationModel, [FromQuery] bool getDeleted)
        {
            if (id != locationModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(locationModel);

            var locationType = await _locationTypeService.GetLocationTypeAsync(locationModel.LocationTypeId, getDeleted);
            if (locationType == null)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }
            var location = _mapper.Map<Location>(locationModel);
            location.LocationType = locationType;

            var (success, errorMessage) = await _locationService.EditLocationAsync(location);
            if (success) return RedirectToAction(nameof(Index));
            if (await _locationService.GetLocationAsync(location.Id) == null) return NotFound();
            await CreateLocationTypeDropdownList(getDeleted);
            TempData["Error"] = errorMessage;
            return View(locationModel);
        }

        // POST: Locations/Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _locationService.DeleteLocationAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Locations/Location/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _locationService.LocationExistsAsync((int)id, true))) return NotFound();
            await _locationService.RestoreLocationAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _locationService.GetLocationsAsync(request);
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

                return new JsonResult(new DataTableResponse<LocationModel>
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

        private async Task CreateLocationTypeDropdownList(bool getDeleted = false, object selectedType = null)
        {
            var locationTypes = await _locationTypeService.GetLocationTypesAsync(getDeleted);
            var ltModels = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            ViewBag.LocationTypes = new SelectList(ltModels, "Id", "Name", selectedType);
        }
    }
}
