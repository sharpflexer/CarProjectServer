using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.API.Controllers.CRUD
{
    [Authorize(Policy = "Create")]
    public class CreateController : Controller
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        public CreateController(ApplicationContext context, IRequestService requestService)
        {
            _context = context;
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

        /// <summary>
        /// Получает все марки автомобилей из БД.
        /// </summary>
        /// <returns>Список марок автомобилей.</returns>
        public JsonResult GetBrands()
        {
            return new(_context.Brands.Include(b => b.Models).AsNoTracking());
        }

        /// <summary>
        /// Получает модели определенной марки автомобилей по ID из БД.
        /// </summary>
        /// <param name="id">ID автомобильной марки.</param>
        /// <returns>Список моделей автомобилей.</returns>
        public JsonResult GetModels(int id)
        {
            return new(_context.Brands.Include(b => b.Models)
                                      .AsNoTracking()
                                      .Single(b => b.Id.Equals(id))
                                      .Models);
        }

        /// <summary>
        /// Получает расцветки автомобилей, доступных для определенной модели по ID из БД.
        /// </summary>
        /// <param name="id">ID модели автомобиля.</param>
        /// <returns></returns>
        public JsonResult GetColors(int id)
        {
            return new(_context.Models.Include(m => m.Colors)
                                      .AsNoTracking()
                                      .Single(m => m.Id.Equals(id))
                                      .Colors);
        }
    }
}
