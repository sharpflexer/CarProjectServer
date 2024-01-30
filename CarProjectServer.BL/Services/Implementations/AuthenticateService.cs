using CarProjectServer.BL.Commands.Cars;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.Authenticate;
using CarProjectServer.BL.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using NLog.Fluent;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для аутентификации пользователей.
    /// </summary>
    public class AuthenticateService : IAuthenticateService
    {
        /// <summary>
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public AuthenticateService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Проводит аутентификацию пользователя по логину и паролю.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Аутентифицированный пользователь.</returns>
        public async Task<UserModel> AuthenticateUser(string login, string password)
        {
            AuthenticateUserQuery authenticateUser = new AuthenticateUserQuery() 
            { 
                Login = login, 
                Password = password 
            };

            return await _mediator.Send(authenticateUser);
        }

        /// <summary>
        /// Удаляет куки.
        /// </summary>
        /// <param name="cookieToRevoke">Строка куки, которое нужно очистить.</param>
        public async Task RevokeCookie(string cookieToRevoke)
        {
            RevokeCookieCommand revokeCookie = new RevokeCookieCommand()
            {
                CookieToRevoke = cookieToRevoke
            };

            await _mediator.Send(revokeCookie);
        }
    }
}