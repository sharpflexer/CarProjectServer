using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Models;

namespace CarProjectServer.BL.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели авто из BL в DAL и обратно.
    /// </summary>
    public class BlCarProfile : Profile
    {
        public BlCarProfile() 
        {
            CreateMap<BrandModel, Brand>().ReverseMap();
            CreateMap<CarColorModel, CarColor>().ReverseMap();
            CreateMap<CarModelTypeModel, DAL.Models.CarModelType>().ReverseMap();
            CreateMap<Models.CarModel, Car>().ReverseMap();
        }
    }
}
