using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для удаления автомобиля из БД.
    /// </summary>
    [Authorize(Policy = "Delete")]
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : Controller
    {
        private readonly ICarService _carService;

        public DeleteController(ICarService requestService)
        {
            _carService = requestService;
        }

        // POST api/delete/post
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            await _carService.DeleteAsync(HttpContext.Request.Form);

            return RedirectToAction("Index", "Read");
        }
    }
}
