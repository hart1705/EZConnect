using AutoMapper;
using PORTAL.DAL.EF.Models;
using PORTAL.WEB.Models.ActionViewModels;
using PORTAL.WEB.Models.ApplicationRoleViewModels;
using PORTAL.WEB.Models.ApplicationUserViewModels;
using PORTAL.WEB.Models.PermissionViewModels;

namespace PORTAL.WEB.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ApplicationPermission, Permission>().ReverseMap();
            CreateMap<ApplicationAction, ApplicationActionModel>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserModel>().ReverseMap();
        }
    }
}
