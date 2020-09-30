using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Data.Entities.Locations;
using WebStorageSystem.Data.Entities.Products;
using WebStorageSystem.Models.LocationModels;
using WebStorageSystem.Models.ProductModels;

namespace WebStorageSystem.Data.MappingProfiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            LocationTypeMapping();
            LocationMapping();
            ManufacturerMapping();
        }

        private void LocationTypeMapping()
        {
            CreateMap<LocationType, LocationTypeModel>();
            CreateMap<LocationType, LocationTypeModel>();
            CreateMap<List<LocationType>, List<LocationTypeModel>>();
            CreateMap<LocationTypeModel, LocationType>();
            CreateMap<List<LocationTypeModel>, List<LocationType>>();
        }

        private void LocationMapping()
        {
            CreateMap<Location, LocationModel>();
            CreateMap<List<Location>, List<LocationModel>>();
            CreateMap<LocationModel, Location>();
            CreateMap<List<LocationModel>, List<Location>>();
        }

        private void ManufacturerMapping()
        {
            CreateMap<Manufacturer, ManufacturerModel>();
            CreateMap<List<Manufacturer>, List<ManufacturerModel>>();
            CreateMap<ManufacturerModel, Manufacturer>();
            CreateMap<List<ManufacturerModel>, List<Manufacturer>>();
        }
    }
}
