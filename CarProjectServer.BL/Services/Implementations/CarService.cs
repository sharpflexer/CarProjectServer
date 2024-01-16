using AutoMapper;
using CarProjectServer.API.ViewModels;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

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
        /// Инициализирует сервис контекстом БД, маппером и логгером.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        /// <param name="mapper">Маппер для маппинга моделей.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
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
        public async Task<CarModel> CreateAsync(CarModel carModel)
        {
            try
            {
                var auto = _mapper.Map<Car>(carModel);
                auto.Brand = _context.Brands.FirstOrDefault(b => b.Id == carModel.Brand.Id);
                auto.Model = _context.Models.FirstOrDefault(m => m.Id == carModel.Model.Id);
                auto.Color = _context.Colors.FirstOrDefault(c => c.Id == carModel.Color.Id);
                auto.Price = carModel.Price;
                var response = _context.Cars.Add(auto);
                await _context.SaveChangesAsync();
                return _mapper.Map<CarModel>(response.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Невозможно добавить автомобиль");
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
                var auto = _context.Cars.FirstOrDefault(car => car.Id == carModel.Id);
                auto.Brand = _context.Brands.FirstOrDefault(b => b.Id == carModel.Brand.Id);
                auto.Model = _context.Models.FirstOrDefault(m => m.Id == carModel.Model.Id);
                auto.Color = _context.Colors.FirstOrDefault(c => c.Id == carModel.Color.Id);
                _context.Cars.Update(auto);
                auto.Price = carModel.Price;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Невозможно изменить пользователя");
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
                var auto = _context.Cars.FirstOrDefault(car => car.Id == carModel.Id);
                _context.Cars.Remove(auto);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Невозможно удалить пользователя");
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
                   .AsNoTracking()
                   .OrderBy(car => car.Id)
                   .ToListAsync();

                return _mapper.Map<List<CarModel>>(cars);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Список автомобилей недоступен");
            }
        }

        /// <summary>
        /// Получает свойства автомобиля: марки, модели и цвета.
        /// </summary>
        /// <returns>Свойства автомобиля.</returns>
        public async Task<CarPropertiesModel> ReadPropertiesAsync()
        {
            var brands = await _context
                .Brands
                .Include(c => c.Models)
                .AsNoTracking()
                .ToListAsync();

            var models = await _context
                .Models
                .Include(m => m.Colors)
                .AsNoTracking()
                .ToListAsync();

            var colors = await _context
                .Colors
                .AsNoTracking()
                .ToListAsync();

            return new CarPropertiesModel
            {
                Brands = _mapper.Map<List<BrandModel>>(brands),
                Models = _mapper.Map<List<CarModelTypeModel>>(models),
                Colors = _mapper.Map<List<CarColorModel>>(colors)
            };
        }
    }
}
