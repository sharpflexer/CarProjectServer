using AutoMapper;
using CarProjectServer.API.Filters;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;

namespace CarProjectServer.API.Controllers.CRUD
{
    /// <summary>
    /// Контроллер для просмотра и изменения пользователей.
    /// </summary>
    [Authorize]
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
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует контроллер сервисом автомобилей.
        /// </summary>
        /// <param name="carService">Сервис для взаимодействия с БД автомобилей.</param>
        /// <param name="mapper">Маппер для маппинга моделей между слоями.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public CarController(ICarService carService, IMapper mapper, ILogger<CarController> logger)
        {
            _carService = carService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Отправляет запрос на добавление автомобиля 
        /// в базу данных через IRequestService.CreateAsync().
        /// </summary>
        // POST api/car/create
        [Authorize(Policy = "Create")]
        [HttpPost("create")]
        public async Task<ActionResult> Create(CarViewModel carViewModel)
        {
            try
            {
                var auto = _mapper.Map<CarModel>(carViewModel);
                await _carService.CreateAsync(auto);

                return Ok();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }

        /// <summary>
        /// Получает список авто из БД.
        /// </summary>
        /// <returns>Список авто из БД.</returns>
        // GET api/car/read
        [Authorize(Policy = "Read")]
        [AcceptFilterAsync]
        [HttpGet("read")]
        public async Task<ActionResult<IEnumerable<CarViewModel>>> Read()
        {
            try
            {
                IEnumerable<CarModel> carModels = await _carService.ReadAsync();
                var auto = _mapper.Map<IEnumerable<CarViewModel>>(carModels);

                return Ok(auto);
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }

        /// <summary>
        /// Обновляет авто в БД.
        /// </summary>
        /// <param name="carViewModel">Авто для обновления.</param>
        /// <returns>200 OK.</returns>
        // PUT api/car/update
        [Authorize(Policy = "Update")]
        [HttpPut("update")]
        public async Task<IActionResult> Update(CarViewModel carViewModel)
        {
            try
            {
                var auto = _mapper.Map<CarModel>(carViewModel);
                await _carService.UpdateAsync(auto);

                return Ok();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }

        /// <summary>
        /// Удаляет авто из БД.
        /// </summary>
        /// <param name="carViewModel">Авто для удаления.</param>
        /// <returns>200 OK.</returns>
        // DELETE api/car/delete
        [Authorize(Policy = "Delete")]
        [HttpDelete("delete")]
        public async Task<ActionResult> Delete(CarViewModel carViewModel)
        {
            try
            {
                var auto = _mapper.Map<CarModel>(carViewModel);
                await _carService.DeleteAsync(auto);

                return Ok();
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Непредвиденная ошибка взаимодействия с сервером.");
            }
        }
    }
}
