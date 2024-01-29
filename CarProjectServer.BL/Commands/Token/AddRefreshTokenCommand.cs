using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Token
{
    public class AddRefreshTokenCommand : IRequest
    {
        public UserModel User { get; set; }
        public string RefreshToken { get; set; }

        public class AddRefreshTokenHandler : IRequestHandler<AddRefreshTokenCommand>
        {
            /// <summary>
            /// Контекст для взаимодействия с БД.
            /// </summary>
            private readonly ApplicationContext _context;

            /// <summary>
            /// Логгер для логирования в файлы ошибок.
            /// Настраивается в NLog.config.
            /// </summary>
            private readonly ILogger _logger;

            /// <summary>
            /// Инициализирует обработчик контекстом Б Д, маппером и логгером.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            /// <param name="mapper">Маппер для маппинга моделей.</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public AddRefreshTokenHandler(ApplicationContext context, ILogger<AddRefreshTokenHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task Handle(AddRefreshTokenCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.Id == command.User.Id);
                    user.RefreshToken = command.RefreshToken;
                    _context.Users.Update(user);

                    await _context.SaveChangesAsync();
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
