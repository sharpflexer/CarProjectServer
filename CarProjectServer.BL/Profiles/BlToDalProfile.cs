using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Areas.Identity.Models;
using CarProjectServer.DAL.Models;

namespace CarProjectServer.BL.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели из BL в DAL.
    /// </summary>
    public class BlToDalProfile : Profile
    {
        public BlToDalProfile() 
        {
            CreateMap<BrandModel, Brand>();
            CreateMap<CarColorModel, CarColor>();
            CreateMap<CarModelModel, DAL.Models.CarModel>();
            CreateMap<Models.CarModel, Car>();
            CreateMap<ErrorModel, Error>();
            CreateMap<JwtTokenModel, JwtToken>();
            CreateMap<RoleModel, Role>();
            CreateMap<UserModel, User>();
        }
    }
}
