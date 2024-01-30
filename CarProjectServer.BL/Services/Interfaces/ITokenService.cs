using CarProjectServer.BL.Models;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для работы с JWT токенами.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Создаёт Access Token.
        /// </summary>
        /// <param name="user">Пользователь для которого создаётся токен.</param>
        /// <returns>Access Token.</returns>
        Task<string> CreateAccessToken(UserModel user);

        /// <summary>
        /// Создаёт Refresh Token.
        /// </summary>
        /// <returns>Refresh Token.</returns>
        Task<string> CreateRefreshToken();

        /// <summary>
        /// Обновляет устаревший токен.
        /// </summary>
        /// <param name="oldToken">Устаревший токен.</param>
        /// <returns>Новый токен.</returns>
        Task<JwtTokenModel> CreateNewToken(JwtTokenModel oldToken);

        /// <summary>
        /// Получает JWT-токен.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>JWT-токен</returns>
        /// <exception cref="ApiException">Внутренняя ошибка сервера.</exception>
        Task<JwtTokenModel> GetJwtToken(string username, string password);
    }
}
