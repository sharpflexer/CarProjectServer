using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для обновления данных автомобиля.
    /// </summary>
    [Authorize(Policy = "Update")]
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly ICarService _carService;

        /// <summary>
        /// Инициализирует контроллер сервисом для отправки запросов в БД.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public UpdateController(ICarService requestService)
        {
            _carService = requestService;
        }

        // POST api/update/post
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _carService.UpdateAsync(HttpContext.Request.Form);

            return RedirectToAction("Index", "Read");
        }
    }
}
