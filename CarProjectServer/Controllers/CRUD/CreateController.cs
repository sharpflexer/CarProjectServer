using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.API.Controllers.CRUD
{
    [Authorize(Policy = "Create")]
    public class CreateController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        public CreateController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Загружает страницу, используя cascadingDDL.js.
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Отправляет запрос на добавление автомобиля в базу данных через IRequestService.CreateAsync().
        /// Требует заполненных списков HttpContext.Request.Form.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await _requestService.CreateAsync(HttpContext.Request.Form);
            return RedirectToAction("Index", "Read");
        }
    }
}
