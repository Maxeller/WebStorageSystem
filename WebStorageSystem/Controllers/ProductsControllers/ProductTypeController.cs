﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebStorageSystem.Data;
using WebStorageSystem.Data.Entities.Products;
using WebStorageSystem.Data.Services.Products;
using WebStorageSystem.Models.ProductModels;

namespace WebStorageSystem.Controllers.ProductsControllers
{
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
        public async Task<IActionResult> Index([FromQuery] bool getDeleted)
        {
            var productType = await _service.GetProductTypesAsync(getDeleted);
            var models = _mapper.Map<ICollection<ProductTypeModel>>(productType);
            return View(models);
        }

        // GET: ProductType/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var productType = await _service.GetProductTypeAsync((int) id, getDeleted);
            var productTypeModel = _mapper.Map<ProductTypeModel>(productType);
            if (productTypeModel == null) return NotFound();

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

            var productType = await _service.GetProductTypeAsync((int) id, getDeleted);
            var productTypeModel = _mapper.Map<ProductTypeModel>(productType);
            if (productTypeModel == null) return NotFound();

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

            try
            {
                await _service.EditProductTypeAsync(productType);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _service.GetProductTypeAsync(productType.Id) == null) return NotFound();
                throw; // TODO: Handle exception
            }
        }

        // POST: ProductType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var affectedRows = await _service.DeleteProductTypeAsync((int) id);
            if (affectedRows == -1) TempData["Error"] = "Product Type cannot be deleted.<br/>It's used as type in existing Product.";
            return RedirectToAction(nameof(Index));
        }

        // POST: ProductType/Restore/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.ProductTypeExistsAsync((int) id, true))) return NotFound();
            await _service.RestoreProductTypeAsync((int) id);
            return RedirectToAction(nameof(Index));
        }
    }
}
