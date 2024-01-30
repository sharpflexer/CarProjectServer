using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using NLog.Fluent;

namespace CarProjectServer.BL.Queries.Authenticate
{
    public class AuthenticateUserQuery : IRequest<UserModel>
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, UserModel>
        {
            /// <summary>
            /// Сервис для работы с пользователями.
            /// </summary>
            private readonly IUserService _userService;

            /// <summary>
            /// Логгер для логирования в файлы ошибок.
            /// Настраивается в NLog.config.
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Инициализирует обработчик сервисом пользователей, маппером и логгером.
            /// </summary>
            /// <param name="userService">Сервис для работы с пользователями.</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public AuthenticateUserHandler(IUserService userService, ILogger<AuthenticateUserHandler> logger)
            {
                _userService = userService;
                _logger = logger;
            }

            public async Task<UserModel> Handle(AuthenticateUserQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var users = await _userService.GetUsers();
                    var currentUser = users.FirstOrDefault(authUser => authUser.Login == query.Login
                        && authUser.Password == query.Password);

                    return currentUser;
                }
                catch (ApiException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Неверный логин и/или пароль");
                }
            }
        }
    }
}
