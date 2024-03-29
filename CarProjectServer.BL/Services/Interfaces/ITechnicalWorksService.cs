﻿using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса технических работ.
    /// </summary>
    public interface ITechnicalWorksService
    {
        /// <summary>
        /// Проверяет, идут ли сейчас технические работы.
        /// </summary>
        /// <returns>
        /// True - технические работы идут,
        /// False - технических работ сейчас нет.
        public bool AreTechnicalWorksNow();

        /// <summary>
        /// Начинает технические работы, с заданной задержкой.
        /// </summary>
        /// <param name="endTime">Время окончания технических работ.</param>
        public Task StartWorks(DateTime endTime);
    }
}
