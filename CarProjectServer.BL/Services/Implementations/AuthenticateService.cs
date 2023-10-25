using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Areas.Identity;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для аутентификации пользователей.
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        /// <summary>
        /// Инициализирует сервис requestService.
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public AuthenticateService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Проводит аутентификацию пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Аутентифицированный пользователь.</returns>
        public async Task<User> AuthenticateUser(string login, string password)
        {
            IEnumerable<User> users = await _requestService.GetUsers();
            User? currentUser = users.FirstOrDefault(authUser => authUser.Login == login &&
            authUser.Password == password);

            return currentUser;
        }

        /// <summary>
        /// Удаляет куки.
        /// </summary>
        /// <param name="cookieToRevoke">Строка куки, которое нужно очистить.</param>
        public void Revoke(string cookieToRevoke)
        {
            User user = _requestService.GetUserByToken(cookieToRevoke);
            user.RefreshToken = null;

            _requestService.UpdateUser(user);
        }
    }
}