using CarProjectServer.BL.Services.Interfaces;
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
    public class UsersController : Controller
    {
        /// <summary>
        /// Сервис для работы с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Инициализирует контроллер сервисом для отправки запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/users/index
        public async Task<IActionResult> Index()
        {
            ViewData["users"] = await _userService.GetUsers();
            ViewBag.Roles = await _userService.GetRolesAsync();

            return View();
        }

        // GET api/users/update
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            await _userService.UpdateUsersAsync(HttpContext.Request.Form);

            return RedirectToAction("Index");
        }

        // GET api/users/delete
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            await _userService.DeleteUsersAsync(HttpContext.Request.Form);

            return RedirectToAction("Index");
        }
    }
}
