﻿using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.API.ViewModels;
using CarProjectServer.BL.Models;

namespace CarProjectServer.API.Profiles
{
    /// <summary>
    /// Профиль маппинга, маппит модели пользователя из API в BL и обратно.
    /// </summary>
    public class ApiUserProfile : Profile
    {
        public ApiUserProfile()
        {
            CreateMap<ErrorViewModel, ErrorModel>().ReverseMap();
            CreateMap<JwtTokenViewModel, JwtTokenModel>().ReverseMap();
            CreateMap<UserModel, UserViewModel>().ReverseMap();
            CreateMap<RoleModel, RoleViewModel>().ReverseMap();
        }
    }
}
