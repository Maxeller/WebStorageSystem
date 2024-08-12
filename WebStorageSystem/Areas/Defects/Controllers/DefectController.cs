using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Defects.Data.Services;
using WebStorageSystem.Areas.Defects.Models;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Services;
using WebStorageSystem.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Defects.Controllers
{
    [Area("Defects")]
    public class DefectController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DefectService _defectService;
        private readonly UnitService _unitService;
        private readonly ImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public DefectController(IWebHostEnvironment hostEnvironment, DefectService defectService, UnitService unitService, ImageService imageService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _hostEnvironment = hostEnvironment;
            _defectService = defectService;
            _unitService = unitService;
            _imageService = imageService;
            _userManager = userManager;
            _mapper = mapper;
        }

        // GET: Defects/Defect/Index
        public IActionResult Index()
        {
            return View(new DefectModel());
        }

        // GET: Defects/Defect/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var defect = await _defectService.GetDefectAsync((int)id, getDeleted);
            if (defect == null) return NotFound();
            var defectModel = _mapper.Map<DefectModel>(defect);

            return View(defectModel);
        }

        // GET: Defects/Defect/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted = false)
        {
            await CreateUnitDropdownList(getDeleted);
            await CreateUserDropdownList(getDeleted);
            return View();
        }
        
        // POST: Defects/Defect/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DefectNumber,UnitId,CausedByUserId,Description,Notes,Image,IsDeleted")] DefectModel defectModel, [FromQuery] bool getDeleted = false)
        {
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(getDeleted, defectModel.UnitId);
                await CreateUserDropdownList(getDeleted, defectModel.CausedByUserId);
                return View(defectModel);
            }

            if (defectModel.Image.ImageFile != null)
            {
                var image = await _imageService.AddImageAsync(defectModel.Image, _hostEnvironment.WebRootPath);
                defectModel.ImageId = image.Id;
            }
            defectModel.Image = null;

            var defect = _mapper.Map<Defect>(defectModel);

            await _defectService.AddDefectAsync(defect);
            return RedirectToAction(nameof(Index));
        }
        
        // GET: Defects/Defect/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var defect = await _defectService.GetDefectAsync((int)id, getDeleted);
            if (defect == null) return NotFound();
            var defectModel = _mapper.Map<DefectModel>(defect);
            await CreateUnitDropdownList(getDeleted, defectModel.Unit.Id);
            return View(defectModel);
        }
        
        // POST: Defects/Defect/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DefectNumber,UnitId,ReportedByUserId,CausedByUserId,Description,Notes,Image,State,CreatedDate,IsDeleted,RowVersion")] DefectModel defectModel, [FromQuery] bool getDeleted)
        {
            if (id != defectModel.Id) return NotFound();
            if (!ModelState.IsValid)
            {
                await CreateUnitDropdownList(getDeleted, defectModel.Unit.Id);
                await CreateUserDropdownList(getDeleted, defectModel.CausedByUser.Id);
                return View(defectModel);
            }

            var defect = _mapper.Map<Defect>(defectModel);

            var (success, errorMessage) = await _defectService.EditDefectAsync(defect);
            if (success) return RedirectToAction(nameof(Index));
            if (await _defectService.GetDefectAsync(defect.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(defectModel);
        }
        
        // POST: Defects/Defect/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _defectService.DeleteDefectAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
        
        // POST: Defects/Defect/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _defectService.DefectExistsAsync((int)id, true))) return NotFound();
            await _defectService.RestoreDefectAsync((int)id);
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _defectService.GetDefectsAsync(request);
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

                return new JsonResult(new DataTableResponse<DefectModel>
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

        private async Task CreateUnitDropdownList(bool getDeleted = false, object selectedUnit = null)
        {
            var units = await _unitService.GetUnitsAsync(getDeleted);
            var unitModels = _mapper.Map<ICollection<UnitModel>>(units);
            ViewBag.Units = new SelectList(unitModels, "Id", "InventoryNumberProduct", selectedUnit);
        }

        private async Task CreateUserDropdownList(bool getDeleted = false, object selectedUser = null)
        {
            var users = await _userManager.Users.ToListAsync();
            var userModels = _mapper.Map<ICollection<ApplicationUserModel>>(users);
            ViewBag.Users = new SelectList(userModels, "Id", "UserName", selectedUser);
        }
    }
}
