using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;

namespace CarProjectServer.API.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели из BL в API.
    /// </summary>
    public class BlToApiProfile : Profile
    {
        public BlToApiProfile()
        {
            CreateMap<BrandModel, UserViewModel>();
            CreateMap<CarColorModel, CarColorViewModel>();
            CreateMap<CarModelModel, CarModelViewModel>();
            CreateMap<CarModel, CarViewModel>();
            CreateMap<ErrorModel, ErrorViewModel>();
            CreateMap<JwtTokenModel, JwtTokenViewModel>();
            CreateMap<RoleModel, RoleViewModel>();
            CreateMap<UserModel, UserViewModel>();
        } 
    }
}
