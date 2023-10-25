using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для просмотра и изменения пользователей.
    /// </summary>
    [Authorize(Policy = "Users")]
    public class UsersController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        /// <summary>
        /// Инициализирует контроллер сервисом для отправки запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public UsersController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Загружает список пользователей на страницу.
        /// </summary>
        /// <returns>Страница с данными автомобилей в таблице.</returns>
        public async Task<IActionResult> IndexAsync()
        {
            ViewData["users"] = await _requestService.GetUsers();
            ViewBag.Roles = await _requestService.GetRolesAsync();

            return View();
        }

        /// <summary>
        /// Отправляет запрос на добавление автомобиля в базу данных через IRequestService.CreateAsync().
        /// Требует заполненных списков HttpContext.Request.Form.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Update()
        {
            await _requestService.UpdateUsersAsync(HttpContext.Request.Form);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Отправляет запрос на удаление автомобиля в базу данных через IRequestService.DeleteAsync().
        /// Требует заполненных списков HttpContext.Request.Form.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete()
        {
            await _requestService.DeleteUsersAsync(HttpContext.Request.Form);

            return RedirectToAction("Index");
        }
    }
}
