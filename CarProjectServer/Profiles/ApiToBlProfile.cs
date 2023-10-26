using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;

namespace CarProjectServer.API.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели из API в BL.
    /// </summary>
    public class ApiToBlProfile : Profile
    {
        public ApiToBlProfile() 
        {
            CreateMap<BrandViewModel, UserModel>();
            CreateMap<CarColorViewModel, CarColorModel>();
            CreateMap<CarModelViewModel, CarModelModel>();
            CreateMap<CarViewModel, CarModel>();
            CreateMap<ErrorViewModel, ErrorModel>();
            CreateMap<JwtTokenViewModel, JwtTokenModel>();
            CreateMap<RoleViewModel, RoleModel>();
            CreateMap<UserViewModel, UserModel>();
        }
    }
}
