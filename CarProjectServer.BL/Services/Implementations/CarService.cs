using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для взаимодействия с автомобилями в БД.
    /// </summary>
    public class CarService : ICarService
    {
        /// <summary>
        /// Контекст для взаимодействия с БД.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Маппер для маппинга моделей.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует ApplicationContext.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        public CarService(ApplicationContext context, IMapper mapper, ILogger<CarService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Отправляет запрос на добавление нового автомобиля в БД через ApplicationContext.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task CreateAsync(CarModel carModel)
        {
            try
            {
                var auto = _mapper.Map<Car>(carModel);
                _context.Cars.Add(auto);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка добавления авто в БД");
            }
        }

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task UpdateAsync(CarModel carModel)
        {
            try
            {
                var auto = _mapper.Map<Car>(carModel);
                _context.Cars.Update(auto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка обновления авто в БД");
            }
        }

        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task DeleteAsync(CarModel carModel)
        {
            try
            {
                var auto = _mapper.Map<Car>(carModel);
                _context.Cars.Remove(auto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка удаления авто из БД");
            }
        }

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public async Task<IEnumerable<CarModel>> ReadAsync()
        {
            try
            {
                var cars = await _context.Cars
                   .Include(car => car.Brand)
                   .Include(car => car.Model)
                   .Include(car => car.Color)
                   .AsNoTracking().OrderBy(car => car.Id).ToListAsync();

                return _mapper.Map<List<CarModel>>(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка чтения авто из БД");
            }
        }
    }
}
