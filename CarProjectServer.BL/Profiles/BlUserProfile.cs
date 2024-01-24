using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Entities;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;

namespace CarProjectServer.BL.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели пользователя из BL в DAL и обратно.
    /// </summary>
    public class BlUserProfile : Profile
    {
        public BlUserProfile()
        {
            CreateMap<ErrorModel, Error>().ReverseMap();
            CreateMap<JwtTokenModel, JwtToken>().ReverseMap();
            CreateMap<RoleModel, Role>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<TechnicalWorkModel, TechnicalWork>().ReverseMap();
        }
    }
}
