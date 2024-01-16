using AutoMapper;
using CarProjectServer.API.Controllers.CRUD;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
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
    public class RegisterController : ControllerBase
    {
        /// <summary>
        /// Маппер для маппинга моделей между слоями.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис для взаимодействия с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует контроллер сервисом пользователей.
        /// </summary>
        /// <param name="mapper">Маппер для маппинга моделей между слоями.</param>
        /// <param name="userService">Сервис для взаимодействия с пользователями в БД.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public RegisterController(IMapper mapper, IUserService userService, ILogger<RegisterController> logger)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Регистрация пользователя в БД.
        /// </summary>
        /// <param name="user">Аккаунт регистрирующегося пользователь.</param>
        /// <returns>200 OK/400 Bad Request.</returns>
        // POST api/register/post
        [HttpPost]
        public async Task<ActionResult> Post(UserViewModel user)
        {
            try
            {
                var userModel = _mapper.Map<UserModel>(user);
                
                var roleModel = await _userService.GetDefaultRole();
                userModel.Role = roleModel;
                
                await _userService.AddUserAsync(userModel);

                return Ok();
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Ошибка регистрации");
            }
        }
    }
}
