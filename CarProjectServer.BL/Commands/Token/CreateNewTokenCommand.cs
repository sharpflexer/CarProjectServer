using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Token
{
    public class CreateNewTokenCommand : IRequest<JwtTokenModel>
    {
        public JwtTokenModel OldToken { get; set; }

        public class CreateNewTokenHandler : IRequestHandler<CreateNewTokenCommand, JwtTokenModel>
        {
            /// <summary>
            /// Сервис для работы с пользователями.
            /// </summary>
            private readonly IUserService _userService;

            /// <summary>
            /// Сервис для работы с токенами.
            /// </summary>
            private readonly ITokenService _tokenService;

            /// <summary>
            /// Логгер для логирования в файлы ошибок.
            /// Настраивается в NLog.config.
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Инициализирует обработчик сервисом пользователей, сервисом токенов и логгером.
            /// </summary>
            /// <param name="userService">Сервис для работы с пользователями.</param>
            /// <param name="tokenService">Сервис для работы с токенами.</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public CreateNewTokenHandler(IUserService userService, ITokenService tokenService, ILogger<CreateNewTokenHandler> logger)
            {
                _userService = userService;
                _tokenService = tokenService;
                _logger = logger;
            }

            public async Task<JwtTokenModel> Handle(CreateNewTokenCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _userService.GetUserByToken(command.OldToken.RefreshToken);

                    var newAccessToken = "Bearer " + await _tokenService.CreateAccessToken(user);
                    var newRefreshToken = await _tokenService.CreateRefreshToken();

                    await _userService.AddRefreshToken(user, newRefreshToken);

                    return new JwtTokenModel
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    };
                }
                catch (ApiException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Ошибка аутентификации");
                }
            }
        }
    }
}
