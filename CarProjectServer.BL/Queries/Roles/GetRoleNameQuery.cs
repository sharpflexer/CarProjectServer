using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Queries.Roles
{
    public class GetRoleNameQuery : IRequest<string>
    {
        public string Username { get; set; }

        public class GetRoleNameHandler : IRequestHandler<GetRoleNameQuery, string>
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
            public GetRoleNameHandler(ApplicationContext context, ILogger<GetRoleNameHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<string> Handle(GetRoleNameQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == query.Username);

                    if (user == null)
                    {
                        throw new ApiException("Пользователь не найден");
                    }

                    return user.Role.Name;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);

                    throw new ApiException("Внутренняя ошибка сервера");
                }
            }
        }
    }
}
