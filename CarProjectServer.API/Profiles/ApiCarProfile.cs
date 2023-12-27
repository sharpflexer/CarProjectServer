using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.API.ViewModels;
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
            CreateMap<BrandViewModel, BrandModel>().ReverseMap();
            CreateMap<CarColorViewModel, CarColorModel>();
            CreateMap<CarModelViewModel, CarModelTypeModel>().ReverseMap();
            CreateMap<CarViewModel, CarModel>().ReverseMap();
            CreateMap<CarPropertiesViewModel, CarPropertiesModel>()
                .ForMember(props => props.Brands, opts => opts.MapFrom(props => props.brands))

        }
    }
}
