using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Queries.Token
{
    public class GetJwtTokenQuery : IRequest<JwtTokenModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class GetJwtTokenHandler : IRequestHandler<GetJwtTokenQuery, JwtTokenModel>
        {
            /// <summary>
            /// Сервис аутентификации.
            /// </summary>
            private readonly IAuthenticateService _authenticateService;

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
            /// Инициализирует обработчик сервисом аутентификации, 
            /// сервисом пользователей, сервисом токенов и логгером.
            /// </summary>
            /// <param name="authenticateService">Сервис аутентификации.</param>
            /// <param name="userService">Сервис для работы с пользователями.</param>
            /// <param name="userService">Сервис для работы с пользователями.</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public GetJwtTokenHandler(IAuthenticateService authenticateService, 
                IUserService userService, 
                ITokenService tokenService, 
                ILogger<GetJwtTokenHandler> logger)
            {
                _authenticateService = authenticateService;
                _userService = userService;
                _tokenService = tokenService;
                _logger = logger;
            }

            public async Task<JwtTokenModel> Handle(GetJwtTokenQuery command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _authenticateService.AuthenticateUser(command.Username, command.Password);
                    var accessToken = await _tokenService.CreateAccessToken(user);
                    var refreshToken = await _tokenService.CreateRefreshToken();

                    await _userService.AddRefreshToken(user, refreshToken);

                    return new JwtTokenModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
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
