using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebStorageSystem.Data.Entities.Identities;
using WebStorageSystem.Data.Entities.Transfers;
using WebStorageSystem.Models;

namespace WebStorageSystem.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            TransferMapping();
            ApplicationUserMapping();
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
    }
}
