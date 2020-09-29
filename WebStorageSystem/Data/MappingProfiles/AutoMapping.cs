using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Data.Entities.Locations;
using WebStorageSystem.Models.LocationModels;

namespace WebStorageSystem.Data.MappingProfiles
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            LocationTypeMapping();
            LocationMapping();
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
    }
}
