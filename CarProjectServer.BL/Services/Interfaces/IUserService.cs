using CarProjectServer.BL.Models;
using System.Security.Claims;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для взаимодействия с пользователями в БД.
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        public Task<RoleModel> GetDefaultRole();

        /// <summary>
        /// Добавляет Refresh Token в таблицу User.
        /// </summary>
        /// <param name="user">Аккаунт пользователя.</param>
        /// <param name="refreshToken">Токен для обновления access token.</param>
        public void AddRefreshToken(UserModel user, string refreshToken);

        /// <summary>
        /// Добавляет пользователя в БД при регистрации.
        /// </summary>
        /// <param name="user">Аккаунт нового пользователя.</param>
        Task AddUserAsync(UserModel user);

        /// <summary>
        /// Получает список всех пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        Task<IEnumerable<UserModel>> GetUsers();

        /// <summary>
        /// Ищет пользователя по RefreshToken.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Найденный пользователь.</returns>
        UserModel GetUserByToken(string refreshToken);

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        Task UpdateUser(UserModel user);

        /// <summary>
        /// Удаляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для удаления.</param>
        Task DeleteUser(UserModel userModel);

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        Task<IEnumerable<RoleModel>> GetRolesAsync();

        /// <summary>
        /// Получает наименование роли по имени пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Наименование роли.</returns>
        Task<string> GetRoleNameAsync(string username);

        /// <summary>
        /// Получает роль на основе прав пользователя.
        /// </summary>
        /// <param name="claims">Права пользователя.</param>
        /// <returns>Роль пользователя.</returns>
        string GetRoleByClaims(IEnumerable<Claim> claims);

        /// <summary>
        /// Получение пользователя по E-Mail.
        /// </summary>
        /// <param name="email">E-mail пользователя.</param>
        /// <returns>Пользователь.</returns>
        Task<UserModel?> TryGetUserByEmailAsync(string email);
    }
}
