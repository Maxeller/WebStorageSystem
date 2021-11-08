using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Areas.Defects.Data.Entities;
using WebStorageSystem.Areas.Defects.Models;

namespace WebStorageSystem.Areas.Defects.Data.Automapper
{
    public class DefectsMappingProfile : Profile
    {
        public DefectsMappingProfile()
        {
            DefectMapping();
        }

        private void DefectMapping()
        {
            CreateMap<Defect, DefectModel>();
            CreateMap<List<Defect>, List<DefectModel>>();
            CreateMap<DefectModel, Defect>();
            CreateMap<List<DefectModel>, List<Defect>>();
        }
    }
}
