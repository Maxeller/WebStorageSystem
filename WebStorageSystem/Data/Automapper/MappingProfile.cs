using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Models;

namespace WebStorageSystem.Data.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            TransferMapping();
            ApplicationUserMapping();
            ImageMapping();
        }

        private void TransferMapping()
        {
            CreateMap<Transfer, TransferModel>();
            CreateMap<List<Transfer>, List<TransferModel>>();
            CreateMap<TransferModel, Transfer>();
            CreateMap<List<TransferModel>, List<Transfer>>();
        }

        private void ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserModel>();
            CreateMap<List<ApplicationUser>, List<ApplicationUserModel>>();
            CreateMap<ApplicationUserModel, ApplicationUser>();
            CreateMap<List<ApplicationUserModel>, List<ApplicationUser>>();
        }

        private void ImageMapping()
        {
            CreateMap<ImageEntity, ImageEntityModel>();
            CreateMap<List<ImageEntity>, List<ImageEntityModel>>();
            CreateMap<ImageEntityModel, ImageEntity>();
            CreateMap<List<ImageEntityModel>, List<ImageEntity>>();
        }
    }
}
