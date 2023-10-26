using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Areas.Identity.Models;
using CarProjectServer.DAL.Models;

namespace CarProjectServer.BL.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели из DAL в BL.
    /// </summary>
    public class DalToBlProfile : Profile
    {
        public DalToBlProfile()
        {
            CreateMap<Brand, BrandModel>();
            CreateMap<CarColor, CarColorModel>();
            CreateMap<DAL.Models.CarModel, CarModelModel>();
            CreateMap<Car, Models.CarModel>();
            CreateMap<Error, ErrorModel>();
            CreateMap<JwtToken, JwtTokenModel>();
            CreateMap<Role, RoleModel>();
            CreateMap<User, UserModel>();
        }
    }
}
