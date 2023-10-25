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
        /// Инициализирует контроллер сервисом запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public RegisterController(IRequestService requestService)
        {
            _requestService = requestService;
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
        public async Task<IActionResult> PostAsync(User user)
        {
            user.Role = _requestService.GetDefaultRole();
            _requestService.AddUserAsync(user);
            return RedirectToAction("Index", "Login");
        }
    }
}
