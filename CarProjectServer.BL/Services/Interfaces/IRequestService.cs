using CarProjectServer.API.Areas.Identity;
using CarProjectServer.API.Models;
using Microsoft.AspNetCore.Http;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для отправки запросов в БД.
    /// </summary>
    public interface IRequestService
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
        public List<Car> Read();

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

        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        public Role GetDefaultRole();

        /// <summary>
        /// Добавляет Refresh Token в таблицу User.
        /// </summary>
        /// <param name="user">Аккаунт пользователя.</param>
        /// <param name="refreshToken">Токен для обновления access token.</param>
        public void AddRefreshToken(User user);

        /// <summary>
        /// Добавляет пользователя в БД при регистрации.
        /// </summary>
        /// <param name="user">Аккаунт нового пользователя.</param>
        Task AddUserAsync(User user);

        /// <summary>
        /// Получает список всех пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        Task<IEnumerable<User>> GetUsers();

        /// <summary>
        /// Ищет пользователя по RefreshToken.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Найденный пользователь.</returns>
        User GetUserByToken(string refreshToken);

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        Task UpdateUser(User user);

        /// <summary>
        /// Удаляет пользователя из таблицы.
        /// </summary>
        /// <param name="form">Данные пользователя.</param>
        Task DeleteUsersAsync(IFormCollection form);

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="form">Данные пользователя.</param>
        Task UpdateUsersAsync(IFormCollection form);

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        Task<IEnumerable<Role>> GetRolesAsync();
    }
}