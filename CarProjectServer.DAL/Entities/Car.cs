﻿namespace CarProjectServer.DAL.Models
{
    /// <summary>
    /// Автомобиль.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Идентификатор автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Марка автомобиля.
        /// </summary>
        public Brand Brand { get; set; }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public CarModelType Model { get; set; }

        /// <summary>
        /// Цвет автомобиля.
        /// </summary>
        public CarColor Color { get; set; }

        /// <summary>
        /// Цена автомобиля.
        /// </summary>
        public double Price { get; set; } = 0;
    }
}