using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Areas.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarProjectMVC.Controllers.Authorization
{
    /// <summary>
    /// Контроллер для аутентификации и авторизации пользователя.
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// Сервис для работы с JWT токенами.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        /// <summary>
        /// Инициализирует контроллер сервисами токенов, аутентификации и запросов в БД.
        /// </summary>
        /// <param name="authenticateService">Сервис для аутентификации пользователей.</param>
        /// <param name="tokenService">Сервис для работы с JWT токенами.</param>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public LoginController(IAuthenticateService authenticateService,
                               ITokenService tokenService,
                               IRequestService requestService)
        {
            _authenticateService = authenticateService;
            _tokenService = tokenService;
            _requestService = requestService;
        }

        /// <summary>
        /// Загружает страницу входа, если юзер не авторизован 
        /// или страницу с таблицей, если авторизован.
        /// </summary>
        public IActionResult Index()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Read");
            return View();
        }

        /// <summary>
        /// Производит выход пользователя из аккаунта.
        /// </summary>
        /// <returns>Перенаправление на страницу входа.</returns>
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            HttpContext.Response.Cookies.Delete("Refresh");
            var refreshCookie = HttpContext.Request.Cookies["Refresh"];
            _authenticateService.Revoke(refreshCookie);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Выполняет авторизацию и перенаправляет на страницу с таблицей, 
        /// если пользователь прошел аутентификацию
        /// или возвращает сообщение, если не прошел.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="isSuccess">Результат проверки.</param>
        /// <returns>
        /// <list type="bullet|number|table">
        /// <listheader>
        /// <description>Новое представление, в зависимости от результата входа.</description>
        /// </listheader>
        /// <item><term>Успешный вход</term><description> /Read/Index.</description></item>
        /// <item><term>Неудачный вход</term><description> BadRequest.</description></item>
        /// </list>
        /// </returns>
        private async Task<object> SignInIfSucceed(string username, User isSuccess)
        {
            if (isSuccess != null)
            {
                ViewBag.username = string.Format("Successfully logged-in", username);
                TempData["username"] = username;
                return RedirectToAction("Index", "Read");
            }
            else
            {
                ViewBag.username = string.Format("Login Failed: {0}", username);
                return BadRequest("Логин и/или пароль не установлены");
            }
        }
    }
}
