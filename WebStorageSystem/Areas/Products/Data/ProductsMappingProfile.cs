using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Areas.Products.Data.Entities;
using WebStorageSystem.Areas.Products.Models;

namespace WebStorageSystem.Areas.Products.Data
{
    public class ProductsMappingProfile : Profile
    {
        public ProductsMappingProfile()
        {
            ManufacturerMapping();
            ProductTypeMapping();
            VendorMapping();
            ProductMapping();
        }

        private void ManufacturerMapping()
        {
            CreateMap<Manufacturer, ManufacturerModel>();
            CreateMap<List<Manufacturer>, List<ManufacturerModel>>();
            CreateMap<ManufacturerModel, Manufacturer>();
            CreateMap<List<ManufacturerModel>, List<Manufacturer>>();
        }

        private void ProductTypeMapping()
        {
            CreateMap<ProductType, ProductTypeModel>();
            CreateMap<List<ProductType>, List<ProductTypeModel>>();
            CreateMap<ProductTypeModel, ProductType>();
            CreateMap<List<ProductTypeModel>, List<ProductType>>();
        }

        private void VendorMapping()
        {
            CreateMap<Vendor, VendorModel>();
            CreateMap<List<Vendor>, List<VendorModel>>();
            CreateMap<VendorModel, Vendor>();
            CreateMap<List<VendorModel>, List<Vendor>>();
        }

        private void ProductMapping()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<List<Product>, List<ProductModel>>();
            CreateMap<ProductModel, Product>();
            CreateMap<List<ProductModel>, List<Product>>();
        }
    }
}
