﻿using CarProjectServer.BL.Models;

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
        /// <param name="carModel">Автомобиль для добавления.</param>
        Task<CarModel> CreateAsync(CarModel carModel);

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        Task<IEnumerable<CarModel>> ReadAsync();

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="carModel">Автомобиль для обновления.</param>
        Task UpdateAsync(CarModel carModel);

        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="carModel">Автомобиль для удаления.</param>
        Task DeleteAsync(CarModel carModel);

        /// <summary>
        /// Получает свойства автомобиля: марки, модели и цвета.
        /// </summary>
        /// <returns>Свойства автомобиля.</returns>
        Task<CarPropertiesModel> ReadPropertiesAsync();
    }
}