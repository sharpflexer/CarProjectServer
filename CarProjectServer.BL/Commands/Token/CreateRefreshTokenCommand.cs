using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace CarProjectServer.BL.Commands.Token
{
    public class CreateRefreshTokenCommand : IRequest<string>
    {
        public class CreateRefreshTokenHandler : IRequestHandler<CreateRefreshTokenCommand, string>
        {
            /// <summary>
            /// Логгер для логирования в файлы ошибок.
            /// Настраивается в NLog.config.
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Инициализирует обработчик логгером.
            /// </summary>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public CreateRefreshTokenHandler(ILogger<CreateRefreshTokenHandler> logger)
            {
                _logger = logger;
            }

            public async Task<string> Handle(CreateRefreshTokenCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var randomNumber = new byte[32];
                    using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                    rng.GetBytes(randomNumber);

                    return Convert.ToBase64String(randomNumber);
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
