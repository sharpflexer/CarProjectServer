using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectMVC.Controllers.Authorization
{
    /// <summary>
    /// Контроллер для регистрации пользователей.
    /// </summary>
    public class RegisterController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        /// <summary>
        /// Маппер для маппинга моделей между слоями
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует контроллер сервисом запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public RegisterController(IRequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _mapper = mapper;
        }

        /// <summary>
        /// Отображает страницу для регистрации.
        /// </summary>
        /// <returns>Страница для регистрации.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Добавляет зарегистрированного пользователя в БД.
        /// </summary>
        /// <param name="user">Зарегистрированный пользователь.</param>
        /// <returns>Перенаправление на страницу входа.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(UserViewModel user)
        {
            var roleModel = _requestService.GetDefaultRole();
            user.Role = _mapper.Map<RoleViewModel>(roleModel);
            var userModel = _mapper.Map<UserModel>(user);
            await _requestService.AddUserAsync(userModel);
            return RedirectToAction("Index", "Login");
        }
    }
}
