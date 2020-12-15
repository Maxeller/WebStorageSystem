using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using JqueryDataTables.ServerSide.AspNetCoreWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    public class ProductController : Controller
    {
        private readonly ProductService _service;
        private readonly ProductTypeService _ptService;
        private readonly ManufacturerService _mService;
        private readonly IMapper _mapper;

        public ProductController(ProductService service, ProductTypeService productTypeService, ManufacturerService manufacturerService, IMapper mapper)
        {
            _service = service;
            _ptService = productTypeService;
            _mService = manufacturerService;
            _mapper = mapper;
        }

        // GET: Products/Product
        public IActionResult Index([FromQuery] bool getDeleted)
        {
            return View(new ProductModel());
        }

        // GET: Products/Product/Details/5
        public async Task<IActionResult> Details(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var product = await _service.GetProductAsync((int)id, getDeleted);
            if (product == null) return NotFound();
            var productModel = _mapper.Map<ProductModel>(product);

            return View(productModel);
        }

        // GET: Products/Product/Create
        public async Task<IActionResult> Create([FromQuery] bool getDeleted)
        {
            await CreateDropdownLists(getDeleted);
            return View();
        }

        // POST: Products/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ProductNumber,Description,Webpage,IsDeleted,ManufacturerId,ProductTypeId")] ProductModel productModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateDropdownLists(getDeleted);
                return View(productModel);
            }

            var manufacturer = await _mService.GetManufacturerAsync(productModel.ManufacturerId, getDeleted);
            var productType = await _ptService.GetProductTypeAsync(productModel.ProductTypeId, getDeleted);
            if (manufacturer == null || productType == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(productModel);
            }
            var product = _mapper.Map<Product>(productModel);
            product.Manufacturer = manufacturer;
            product.ProductType = productType;
            await _service.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Product/Edit/5
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted)
        {
            if (id == null) return BadRequest();

            var product = await _service.GetProductAsync((int)id, getDeleted);
            if (product == null) return NotFound();
            var productModel = _mapper.Map<ProductModel>(product);

            await CreateDropdownLists(getDeleted, productModel.Manufacturer, productModel.ProductType);

            return View(productModel);
        }

        // POST: Products/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ProductNumber,Description,Webpage,Id,ManufacturerId,ProductTypeId,CreatedDate,IsDeleted,RowVersion")] ProductModel productModel, [FromQuery] bool getDeleted)
        {
            if (id != productModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(productModel);

            var manufacturer = await _mService.GetManufacturerAsync(productModel.ManufacturerId, getDeleted);
            var productType = await _ptService.GetProductTypeAsync(productModel.ProductTypeId, getDeleted);
            if (manufacturer == null || productType == null)
            {
                await CreateDropdownLists(getDeleted);
                return View(productModel);
            }
            var product = _mapper.Map<Product>(productModel);
            product.Manufacturer = manufacturer;
            product.ProductType = productType;

            var (success, errorMessage) = await _service.EditProductAsync(product);
            if (success) return RedirectToAction(nameof(Index));

            if (await _service.GetProductAsync(product.Id) == null) return NotFound();
            await CreateDropdownLists(getDeleted);
            TempData["Error"] = errorMessage;
            return View(productModel);
        }

        // POST: Products/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _service.DeleteProductAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Product/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _service.ProductExistsAsync((int)id, true))) return NotFound();
            await _service.RestoreProductAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] JqueryDataTablesParameters param)
        {
            try
            {
                HttpContext.Session.SetString(nameof(JqueryDataTablesParameters), JsonSerializer.Serialize(param));
                var results = await _service.GetProductsAsync(param);
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

                return new JsonResult(new JqueryDataTablesResult<ProductModel>
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

        private async Task CreateDropdownLists(bool getDeleted = false, object selectedManufacturer = null, object selectedType = null)
        {
            await CreateManufacturerDropdownList(getDeleted, selectedManufacturer);
            await CreateProductTypeDropdownList(getDeleted, selectedType);
        }

        private async Task CreateManufacturerDropdownList(bool getDeleted = false, object selectedManufacturer = null)
        {
            var manufacturers = await _mService.GetManufacturersAsync(getDeleted);
            var mModels = _mapper.Map<ICollection<ManufacturerModel>>(manufacturers);
            ViewBag.Manufacturers = new SelectList(mModels, "Id", "Name", selectedManufacturer);
        }

        private async Task CreateProductTypeDropdownList(bool getDeleted = false, object selectedType = null)
        {
            var productTypes = await _ptService.GetProductTypesAsync(getDeleted);
            var ptModels = _mapper.Map<ICollection<ProductTypeModel>>(productTypes);
            ViewBag.ProductTypes = new SelectList(ptModels, "Id", "Name", selectedType);
        }
    }
}
