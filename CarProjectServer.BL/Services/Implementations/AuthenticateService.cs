using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для аутентификации пользователей.
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        /// <summary>
        /// Сервис для работы с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует сервис сервисом для взаимодействия с пользователями в БД.
        /// </summary>
        /// <param name="userService">Сервис для отправки запросов в БД.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public AuthenticateService(IUserService userService, ILogger<AuthenticateService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Проводит аутентификацию пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Аутентифицированный пользователь.</returns>
        public async Task<UserModel> AuthenticateUser(string login, string password)
        {
            try
            {
                var users = await _userService.GetUsers();
                var currentUser = users.FirstOrDefault(authUser => authUser.Login == login &&
                    authUser.Password == password);

                return currentUser;
            }
            catch(ApiException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Неверный логин и/или пароль");
            }
        }

        /// <summary>
        /// Удаляет куки.
        /// </summary>
        /// <param name="cookieToRevoke">Строка куки, которое нужно очистить.</param>
        public void Revoke(string cookieToRevoke)
        {
            try
            {
                var user = _userService.GetUserByToken(cookieToRevoke);
                user.RefreshToken = null;

                _userService.UpdateUser(user);
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Не удалось выйти из аккаунта");
            }
        }
    }
}