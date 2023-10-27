using CarProjectServer.API.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для просмотра списка автомобилей.
    /// </summary>
    [Authorize(Policy = "Read")]
    [Route("api/[controller]")]
    [ApiController]
    public class ReadController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly ICarService _carService;

        /// <summary>
        /// Инициализирует контроллер сервисом для отправки запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public ReadController(ICarService requestService)
        {
            _carService = requestService;
        }

        // GET api/read/index
        [HttpGet]
        public IActionResult Index()
        {
            ViewData["cars"] = _carService.Read();

            return View();
        }

        /// <summary>
        /// Показывает ошибку.
        /// </summary>
        /// <returns>Страница с ошибкой.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}