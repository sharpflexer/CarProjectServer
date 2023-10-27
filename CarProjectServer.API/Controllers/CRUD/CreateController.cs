using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для создания автомобиля и добавления в БД.
    /// </summary>
    [Authorize(Policy = "Create")]
    [Route("api/[controller]")]
    [ApiController]
    public class CreateController : Controller
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly ICarService _carService;

        public CreateController(ICarService requestService)
        {
            _carService = requestService;
        }

        // GET api/create/index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // GET api/create/post
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _carService.CreateAsync(HttpContext.Request.Form);

            return RedirectToAction("Index", "Read");
        }
    }
}
