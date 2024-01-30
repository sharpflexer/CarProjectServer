using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса технических работ.
    /// </summary>
    public interface ITechnicalWorkService
    {
        /// <summary>
        /// Проверяет, идут ли сейчас технические работы.
        /// </summary>
        /// <returns>
        /// True - технические работы идут,
        /// False - технических работ сейчас нет.
        Task<bool> CheckTechnicalWork();

        /// <summary>
        /// Начинает технические работы, с заданной задержкой.
        /// </summary>
        /// <param name="endTime">Время окончания технических работ.</param>
        Task StartTechnicalWork(DateTime endTime);
    }
}
