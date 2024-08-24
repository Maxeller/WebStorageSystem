using System.Collections.Generic;
using AutoMapper;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Models;
using WebStorageSystem.Models.Transfers;

namespace WebStorageSystem.Data.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MainTransferMapping();
            SubTransferMapping();
            ApplicationUserMapping();
            ImageMapping();
        }

        private void MainTransferMapping()
        {
            CreateMap<MainTransfer, MainTransferModel>();
            CreateMap<List<MainTransfer>, List<MainTransferModel>>();
            CreateMap<MainTransferModel, MainTransfer>();
            CreateMap<List<MainTransferModel>, List<MainTransfer>>();
        }

        private void SubTransferMapping()
        {
            CreateMap<SubTransfer, SubTransferModel>();
            CreateMap<List<SubTransfer>, List<SubTransferModel>>();
            CreateMap<SubTransferModel, SubTransfer>();
            CreateMap<List<SubTransferModel>, List<SubTransfer>>();
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
