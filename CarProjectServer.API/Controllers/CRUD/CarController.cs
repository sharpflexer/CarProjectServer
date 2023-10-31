using AutoMapper;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace CarProjectServer.API.Controllers.CRUD
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        /// <summary>
        /// Сервис для взаимодействия с БД автомобилей.
        /// </summary>
        private readonly ICarService _carService;

        /// <summary>
        /// Маппер для маппинга моделей
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует контроллер сервисом автомобилей.
        /// </summary>
        /// <param name="carService"></param>
        public CarController(ICarService carService, IMapper mapper)
        {
            _carService = carService;
            _mapper = mapper;
        }

        /// <summary>
        /// Отправляет запрос на добавление автомобиля 
        /// в базу данных через IRequestService.CreateAsync().
        /// </summary>
        // POST api/car/create
        [HttpPost("create")]
        public async Task<ActionResult> Create(CarViewModel carViewModel)
        {
            var auto = _mapper.Map<CarModel>(carViewModel);
            await _carService.CreateAsync(auto);

            return Ok();
        }

        /// <summary>
        /// Получает список авто из БД.
        /// </summary>
        /// <returns>Список авто из БД.</returns>
        // GET api/car/read
        [HttpGet("read")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> Read()
        {
            var carModels = await _carService.ReadAsync();
            var auto = _mapper.Map<IEnumerable<CarViewModel>>(carModels);

            return Ok(auto);
        }

        /// <summary>
        /// Обновляет авто в БД.
        /// </summary>
        /// <param name="carViewModel">Авто для обновления.</param>
        /// <returns>200 OK.</returns>
        // PUT api/car/update
        [HttpPut("update")]
        public async Task<IActionResult> Update(CarViewModel carViewModel)
        {
            var auto = _mapper.Map<CarModel>(carViewModel);
            await _carService.UpdateAsync(auto);

            return Ok();
        }

        /// <summary>
        /// Удаляет авто из БД.
        /// </summary>
        /// <param name="carViewModel">Авто для удаления.</param>
        /// <returns>200 OK.</returns>
        // DELETE api/car/delete
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(CarViewModel carViewModel)
        {
            var auto = _mapper.Map<CarModel>(carViewModel);
            await _carService.DeleteAsync(auto);

            return Ok();
        }

        /// <summary>
        /// Показывает ошибку.
        /// </summary>
        /// <returns>Страница с ошибкой.</returns>
        // GET api/car/error
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult<ErrorViewModel> Error()
        {
            return Ok(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
