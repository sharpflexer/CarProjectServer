﻿using CarProjectServer.API.Models;

namespace CarProjectServer.API.Controllers.Models
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
        public CarModel Model { get; set; }

        /// <summary>
        /// Цвет автомобиля.
        /// </summary>
        public CarColor Color { get; set; }
    }
}
