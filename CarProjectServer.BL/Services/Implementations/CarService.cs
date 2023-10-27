using AutoMapper;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

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
        /// Маппер для маппинга моделей
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует ApplicationContext.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        public CarService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Отправляет запрос на добавление нового автомобиля в БД через ApplicationContext.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task CreateAsync(IFormCollection form)
        {
            var auto = new Car()
            {
                Brand = _context.Brands.Single(brand => brand.Id == int.Parse(form["Brands"])),
                Model = _context.Models.Single(model => model.Id == int.Parse(form["Models"])),
                Color = _context.Colors.Single(color => color.Id == int.Parse(form["Colors"])),
            };

            _context.Cars.Add(auto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task UpdateAsync(IFormCollection form)
        {
            var auto = new Car()
            {
                Id = int.Parse(form["IDs"]),
                Brand = _context.Brands.Single(brand => brand.Id == int.Parse(form["Brands"])),
                Model = _context.Models.Single(model => model.Id == int.Parse(form["Models"])),
                Color = _context.Colors.Single(color => color.Id == int.Parse(form["Colors"])),
            };
            _context.Cars.Update(auto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task DeleteAsync(IFormCollection form)
        {
            var auto = new Car()
            {
                Id = int.Parse(form["IDs"]),
            };

            _context.Cars.Remove(auto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public async Task<List<Models.CarModel>> Read()
        {
            var cars = await _context.Cars
               .Include(car => car.Brand)
               .Include(car => car.Model)
               .Include(car => car.Color)
               .AsNoTracking().OrderBy(car => car.Id).ToListAsync();

            return _mapper.Map<List<Models.CarModel>>(cars);
        }
    }
}
