using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Areas.Locations.Data.Entities;
using WebStorageSystem.Areas.Locations.Models;

namespace WebStorageSystem.Areas.Locations.Data.Automapper
{
    public class LocationsMappingProfile : Profile
    {
        public LocationsMappingProfile()
        {
            LocationTypeMapping();
            LocationMapping();
        }

        private void LocationTypeMapping()
        {
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
    }
}
