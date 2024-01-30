using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Extensions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;

namespace CarProjectServer.BL.Commands.Token
{
    public class CreateAccessTokenCommand : IRequest<string>
    {
        public UserModel User { get; set; }

        public class CreateAccessTokenHandler : IRequestHandler<CreateAccessTokenCommand, string>
        {
            /// <summary>
            /// Логгер для логирования в файлы ошибок.
            /// Настраивается в NLog.config.
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Инициализирует обработчик контекстом Б Д, маппером и логгером.
            /// </summary>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public CreateAccessTokenHandler(ILogger<CreateAccessTokenHandler> logger)
            {
                _logger = logger;
            }

            public async Task<string> Handle(CreateAccessTokenCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var token = command.User
                        .CreateClaims()
                        .CreateJwtToken();
                    JwtSecurityTokenHandler tokenHandler = new();

                    return tokenHandler.WriteToken(token);
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
