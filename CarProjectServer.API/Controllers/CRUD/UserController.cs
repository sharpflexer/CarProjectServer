using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для просмотра и изменения пользователей.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// Сервис для взаимодействия с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Маппер для маппинга моделей.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует контроллер сервисом пользователей. 
        /// </summary>
        /// <param name="userService">Сервис для взаимодействия с пользователями в БД.</param>
        /// <param name="mapper">Маппер для маппинга моделей.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получает список пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        // GET api/user/read
        [Authorize("Users")]
        [HttpGet("read")]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Read()
        {
            try
            {
                var users = await _userService.GetUsers();
                var userViews = _mapper.Map<IEnumerable<UserViewModel>>(users);

                return Ok(userViews);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }

        /// <summary>
        /// Отправляет запрос на обновление пользователя 
        /// в базу данных через IRequestService.CreateAsync().
        /// </summary>
        /// <returns>200 OK.</returns>
        // PUT api/user/update
        [Authorize("Users")]
        [HttpPut("update")]
        public async Task<ActionResult> Update(UserViewModel userViewModel)
        {
            try
            {
                var user = _mapper.Map<UserModel>(userViewModel);
                await _userService.UpdateUser(user);

                return Ok();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }

        /// <summary>
        /// Отправляет запрос на добавление автомобиля
        /// в базу данных через IRequestService.CreateAsync().
        /// </summary>
        /// <returns>200 OK.</returns>
        // DELETE api/user/delete
        [Authorize("Users")]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(UserViewModel userViewModel)
        {
            try
            {
                var user = _mapper.Map<UserModel>(userViewModel);
                await _userService.DeleteUser(user);

                return Ok();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }
    }
}
