using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectMVC.Controllers.Authorization
{
    /// <summary>
    /// Контроллер для выхода из аккаунта
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogOutController : Controller
    {
        /// <summary>
        /// Сервис для аутентификации пользователей
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        /// <summary>
        /// Инициализирует контроллер сервисом аутентификации
        /// </summary>
        /// <param name="authenticateService">Сервис для аутентификации пользователей</param>
        public LogOutController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        // GET api/logout/index
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Response.Cookies.Delete("Refresh");
            HttpContext.Response.Headers.Remove("Authentication");
            string? refreshCookie = HttpContext.Request.Cookies["Refresh"];
            _authenticateService.Revoke(refreshCookie);
            return RedirectToAction("Index", "Login");
        }
    }
}
