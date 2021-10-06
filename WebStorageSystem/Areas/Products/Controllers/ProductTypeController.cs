using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductTypeController : Controller
    {
        private readonly ProductTypeService _service;
        private readonly IMapper _mapper;

        public ProductTypeController(ProductTypeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: ProductType
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new ProductTypeModel());
        }

        // GET: ProductType/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var productType = await _service.GetProductTypeAsync((int)id, getDeleted);
            if (productType == null) return NotFound();
            var productTypeModel = _mapper.Map<ProductTypeModel>(productType);

            return View(productTypeModel);
        }

        // GET: ProductType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,IsDeleted")] ProductTypeModel productTypeModel)
        {
            if (!ModelState.IsValid) return View(productTypeModel);

            var productType = _mapper.Map<ProductType>(productTypeModel);
            await _service.AddProductTypeAsync(productType);
            return RedirectToAction(nameof(Index));
        }

        // GET: ProductType/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var productType = await _service.GetProductTypeAsync((int)id, getDeleted);
            if (productType == null) return NotFound();
            var productTypeModel = _mapper.Map<ProductTypeModel>(productType);

            return View(productTypeModel);
        }

        // POST: ProductType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,Id,CreatedDate,IsDeleted,RowVersion")] ProductTypeModel productTypeModel)
        {
            if (id != productTypeModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(productTypeModel);

            var productType = _mapper.Map<ProductType>(productTypeModel);

            var (success, errorMessage) = await _service.EditProductTypeAsync(productType);
            if (success) return RedirectToAction(nameof(Index));
            if (await _service.GetProductTypeAsync(productType.Id) == null) return NotFound();
            TempData["Error"] = errorMessage;
            return View(productTypeModel);
        }

        // POST: ProductType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            (bool success, string errorMessage) = await _service.DeleteProductTypeAsync((int)id);
            if (!success) TempData["Error"] = errorMessage;
            return RedirectToAction(nameof(Index));
        }

        // POST: ProductType/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.ProductTypeExistsAsync((int)id, true))) return NotFound();
            await _service.RestoreProductTypeAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _service.GetProductTypesAsync(request);
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

                return new JsonResult(new DataTableResponse<ProductTypeModel>
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