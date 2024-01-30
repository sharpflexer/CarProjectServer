using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class RevokeCookieCommand : IRequest
    {
        public string CookieToRevoke { get; set; }

        public class RevokeCookieHandler : IRequestHandler<RevokeCookieCommand>
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
            /// Инициализирует обработчик сервисом пользователей и логгером.
            /// </summary>
            /// <param name="userService">Сервис для работы с пользователями..</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public RevokeCookieHandler(IUserService userService, ILogger<RevokeCookieHandler> logger)
            {
                _userService = userService;
                _logger = logger;
            }

            public async Task Handle(RevokeCookieCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _userService.GetUserByToken(command.CookieToRevoke);
                    user.RefreshToken = null;

                    await _userService.UpdateUser(user);
                }
                catch (ApiException)
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
}
