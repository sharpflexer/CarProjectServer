using AutoMapper;
using CarProjectServer.API.Models;
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
    [Authorize(Policy = "Users")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        /// <summary>
        /// Сервис для работы с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Маппер для маппинга моделей
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует контроллер сервисом для отправки запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET api/users/read
        public async Task<ActionResult<IEnumerable<UserViewModel>>> Read()
        {
            var users = await _userService.GetUsers();
            var userViews = _mapper.Map<IEnumerable<UserViewModel>>(users);

            return Ok(userViews);
        }

        // GET api/users/update
        [HttpPost]
        public async Task<ActionResult> Update(UserViewModel userViewModel)
        {
            var user = _mapper.Map<UserModel>(userViewModel);
            await _userService.UpdateUser(user);

            return Ok();
        }

        // GET api/users/delete
        [HttpPost]
        public async Task<ActionResult> Delete(UserViewModel userViewModel)
        {
            var user = _mapper.Map<UserModel>(userViewModel);
            await _userService.DeleteUser(user);

            return Ok();
        }
    }
}
