using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;

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
        /// Инициализирует сервис requestService.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public AuthenticateService(IUserService userService)
        {
            _userService = userService;
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
            catch(Exception ex)
            {
                throw new ApiException();
            }
        }

        /// <summary>
        /// Удаляет куки.
        /// </summary>
        /// <param name="cookieToRevoke">Строка куки, которое нужно очистить.</param>
        public void Revoke(string cookieToRevoke)
        {
            var user = _userService.GetUserByToken(cookieToRevoke);
            user.RefreshToken = null;

            _userService.UpdateUser(user);
        }
    }
}