using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarProjectServer.API.Controllers.CRUD
{
    [Authorize(Policy = "Delete")]
    public class DeleteController : Controller
    {
        private readonly IRequestService _requestService;

        public DeleteController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Отправляет запрос на удаление автомобиля в базу данных через IRequestService.DeleteAsync().
        /// Требует заполненных списков HttpContext.Request.Form.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            await _requestService.DeleteAsync(HttpContext.Request.Form);
            return RedirectToAction("Index", "Read");
        }
    }
}
