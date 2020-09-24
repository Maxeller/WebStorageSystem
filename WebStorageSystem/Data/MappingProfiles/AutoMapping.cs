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
            CreateMap<LocationType, LocationTypeModel>();
            CreateMap<List<LocationType>, List<LocationTypeModel>>();
            CreateMap<LocationTypeModel, LocationType>();
            CreateMap<List<LocationTypeModel>, List<LocationType>>();
        }
    }
}
