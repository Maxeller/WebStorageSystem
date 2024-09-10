using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Areas.Products.Models;
using WebStorageSystem.Data.Services;
using WebStorageSystem.Models.DataTables;

namespace WebStorageSystem.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize(Roles = "Admin, Warehouse, User")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ProductService _productService;
        private readonly ProductTypeService _productTypeService;
        private readonly ManufacturerService _manufacturerService;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public ProductController(IWebHostEnvironment hostEnvironment, ProductService productService, ProductTypeService productTypeService, ManufacturerService manufacturerService, ImageService imageService, IMapper mapper)
        {
            _hostEnvironment = hostEnvironment;
            _productService = productService;
            _productTypeService = productTypeService;
            _manufacturerService = manufacturerService;
            _imageService = imageService;
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

            var product = await _productService.GetProductAsync((int)id, getDeleted);
            if (product == null) return NotFound();
            var productModel = _mapper.Map<ProductModel>(product);

            return View(productModel);
        }

        // GET: Products/Product/Create
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Create([FromQuery] bool getDeleted)
        {
            await CreateDropdownLists(getDeleted);
            return View();
        }

        // POST: Products/Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Create([Bind("Name,ProductNumber,Description,Webpage,IsDeleted,ManufacturerId,ProductTypeId,Image")] ProductModel productModel, [FromQuery] bool getDeleted)
        {
            if (!ModelState.IsValid)
            {
                await CreateDropdownLists(getDeleted);
                return View(productModel);
            }

            if (productModel.Image.ImageFile != null)
            {
                var image = await _imageService.AddImageAsync(productModel.Image, _hostEnvironment.WebRootPath);
                productModel.ImageId = image.Id;
            }
            productModel.Image = null;

            var product = _mapper.Map<Product>(productModel);

            await _productService.AddProductAsync(product);
            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Product/Edit/5
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int? id, [FromQuery] bool getDeleted = false)
        {
            if (id == null) return BadRequest();

            var product = await _productService.GetProductAsync((int)id, getDeleted);
            if (product == null) return NotFound();
            var productModel = _mapper.Map<ProductModel>(product);

            await CreateDropdownLists(getDeleted, productModel.Manufacturer, productModel.ProductType);

            return View(productModel);
        }

        // POST: Products/Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,ProductNumber,Description,Webpage,IsDeleted,ManufacturerId,ProductTypeId,Image,ImageId,Id,CreatedDate,IsDeleted,RowVersion")] ProductModel productModel, [FromQuery] bool getDeleted = false)
        {
            if (id != productModel.Id) return NotFound();
            if (!ModelState.IsValid) return View(productModel);

            if (productModel.Image.ImageFile != null)
            {
                var image = await _imageService.AddImageAsync(productModel.Image, _hostEnvironment.WebRootPath);
                productModel.ImageId = image.Id;
            }
            productModel.Image = null;
            var product = _mapper.Map<Product>(productModel);

            var (success, errorMessage) = await _productService.EditProductAsync(product);
            if (success) return RedirectToAction(nameof(Index));

            if (await _productService.GetProductAsync(product.Id) == null) return NotFound();
            await CreateDropdownLists(getDeleted);
            TempData["Error"] = errorMessage;
            return View(productModel);
        }

        // POST: Products/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            await _productService.DeleteProductAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        // POST: Products/Product/Delete/5
        [HttpPost, ActionName("Restore")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Warehouse")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            if (!(await _productService.ProductExistsAsync((int)id, true))) return NotFound();
            await _productService.RestoreProductAsync((int)id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadTable(DataTableRequest request)
        {
            try
            {
                var results = await _productService.GetProductsAsync(request);
                foreach (var item in results.Data)
                {
                    var routeValues = new RouteValueDictionary { { "id", item.Id }, { "getDeleted", item.IsDeleted } };
                    item.Action = new Dictionary<string, string>
                    {
                        {"Edit", Url.Action(nameof(Edit), routeValues)},
                        {"Details", Url.Action(nameof(Details), routeValues)},
                        {"Delete", Url.Action(nameof(Delete), new {item.Id})},
                        {"Restore", Url.Action(nameof(Restore), new {item.Id})}
                    };
                }

                return new JsonResult(new DataTableResponse<ProductModel>
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

        private async Task CreateDropdownLists(bool getDeleted = false, object selectedManufacturer = null, object selectedType = null)
        {
            await CreateManufacturerDropdownList(getDeleted, selectedManufacturer);
            await CreateProductTypeDropdownList(getDeleted, selectedType);
        }

        private async Task CreateManufacturerDropdownList(bool getDeleted = false, object selectedManufacturer = null)
        {
            var manufacturers = await _manufacturerService.GetManufacturersAsync(getDeleted);
            var mModels = _mapper.Map<ICollection<ManufacturerModel>>(manufacturers);
            ViewBag.Manufacturers = new SelectList(mModels, "Id", "Name", selectedManufacturer);
        }

        private async Task CreateProductTypeDropdownList(bool getDeleted = false, object selectedType = null)
        {
            var productTypes = await _productTypeService.GetProductTypesAsync(getDeleted);
            var ptModels = _mapper.Map<ICollection<ProductTypeModel>>(productTypes);
            ViewBag.ProductTypes = new SelectList(ptModels, "Id", "Name", selectedType);
        }
    }
}
