using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectMVC.Controllers.Authorization
{
    /// <summary>
    /// Контроллер для регистрации пользователей.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {

        /// <summary>
        /// Маппер для маппинга моделей между слоями
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис для взаимодействия с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Инициализирует контроллер сервисом запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public RegisterController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Добавляет зарегистрированного пользователя в БД.
        /// </summary>
        /// <param name="user">Зарегистрированный пользователь.</param>
        /// <returns>200 OK.</returns>
        // POST api/register/post
        [HttpPost]
        public async Task<ActionResult> Post(UserViewModel user)
        {
            var roleModel = _userService.GetDefaultRole();
            user.Role = _mapper.Map<RoleViewModel>(roleModel);
            var userModel = _mapper.Map<UserModel>(user);
            await _userService.AddUserAsync(userModel);

            return Ok();
        }
    }
}
