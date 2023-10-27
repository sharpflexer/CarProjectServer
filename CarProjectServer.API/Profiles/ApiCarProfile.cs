using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;

namespace CarProjectServer.API.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели авто из API в BL и обратно.
    /// </summary>
    public class ApiCarProfile : Profile
    {
        public ApiCarProfile() 
        {
            CreateMap<BrandViewModel, UserModel>().ReverseMap();
            CreateMap<CarColorViewModel, CarColorModel>().ReverseMap();
            CreateMap<CarModelViewModel, CarModelTypeModel>().ReverseMap();
            CreateMap<CarViewModel, CarModel>().ReverseMap();
        }
    }
}
