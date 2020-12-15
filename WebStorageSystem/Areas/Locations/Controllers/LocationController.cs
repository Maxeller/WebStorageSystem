using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Http;
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

        // GET: Locations/Location
        public IActionResult Index([FromQuery] bool getDeleted = false)
        {
            return View(new LocationModel());
        }

        // GET: Locations/Location/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _service.GetLocationAsync((int)id, getDeleted);
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

        // GET: Locations/Location/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var location = await _service.GetLocationAsync((int)id, getDeleted);
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

            var locationType = await _ltService.GetLocationTypeAsync(locationModel.LocationTypeId, getDeleted);
            if (locationType == null)
            {
                await CreateLocationTypeDropdownList(getDeleted);
                return View(locationModel);
            }
            var location = _mapper.Map<Location>(locationModel);
            location.LocationType = locationType;

            var (success, errorMessage) = await _service.EditLocationAsync(location);
            if (success) return RedirectToAction(nameof(Index));
            if (await _service.GetLocationAsync(location.Id) == null) return NotFound();
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
            await _service.DeleteLocationAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Locations/Location/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.LocationExistsAsync((int)id, true))) return NotFound();
            await _service.RestoreLocationAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonSerializer.Serialize(param));
                var results = await _service.GetLocationsAsync(param);
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

                return new JsonResult(new JqueryDataTablesResult<LocationModel>
                {
                    Draw = param.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
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
            var locationTypes = await _ltService.GetLocationTypesAsync(getDeleted);
            var ltModels = _mapper.Map<ICollection<LocationTypeModel>>(locationTypes);
            ViewBag.LocationTypes = new SelectList(ltModels, "Id", "Name", selectedType);
        }
    }
}
