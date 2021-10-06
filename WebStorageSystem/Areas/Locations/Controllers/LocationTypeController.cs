using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Locations.Models;
using WebStorageSystem.Models.DataTables;

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

        // GET: Locations/LocationType
        public IActionResult Index([FromQuery] bool getDeleted = false)
        {
            return View(new LocationTypeModel());
        }

        // GET: Locations/LocationType/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var locationType = await _service.GetLocationTypeAsync((int)id, getDeleted);
            if (locationType == null) return NotFound();
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);

            return View(locationTypeModel);
        }

        // GET: Locations/LocationType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/LocationType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsDeleted")] LocationTypeModel locationTypeModel)
        {
            if (!ModelState.IsValid) return View(locationTypeModel);

            var locationType = _mapper.Map<LocationType>(locationTypeModel);
            await _service.AddLocationTypeAsync(locationType);
            return RedirectToAction(nameof(Index));
        }

        // GET: Locations/LocationType/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var locationType = await _service.GetLocationTypeAsync((int)id, getDeleted);
            if (locationType == null) return NotFound();
            var locationTypeModel = _mapper.Map<LocationTypeModel>(locationType);

            return View(locationTypeModel);
        }

        // POST: Locations/LocationType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedDate,IsDeleted,RowVersion")] LocationTypeModel locationTypeModel)
        {
            if (id != locationTypeModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(locationTypeModel);

            var locationType = _mapper.Map<LocationType>(locationTypeModel);

            var (success, errorMessage) = await _service.EditLocationTypeAsync(locationType);
            if (success) return RedirectToAction(nameof(Index));
            if (await _service.GetLocationTypeAsync(locationType.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(locationTypeModel);
        }

        // POST: Locations/LocationType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _service.DeleteLocationTypeAsync((int)id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: Locations/LocationType/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.LocationTypeExistsAsync((int)id, true))) return NotFound();
            await _service.RestoreLocationTypeAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _service.GetLocationTypesAsync(request);
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

                return new JsonResult(new DataTableResponse<LocationTypeModel>
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
