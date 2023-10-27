using CarProjectServer.BL.Models;
using Microsoft.AspNetCore.Http;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для взаимодействия с автомобилями в БД.
    /// </summary>
    public interface ICarService
    {
        /// <summary>
        /// Отправляет запрос на добавление нового автомобиля в БД через ApplicationContext.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public Task CreateAsync(IFormCollection form);

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public Task<List<CarModel>> Read();

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public Task UpdateAsync(IFormCollection form);

        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public Task DeleteAsync(IFormCollection form);
    }
}