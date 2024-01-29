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
    public class GetDefaultRoleQuery : IRequest<RoleModel>
    {
        public class GetDefaultRoleHandler : IRequestHandler<GetDefaultRoleQuery, RoleModel>
        {
            /// <summary>
            /// Контекст для взаимодействия с БД.
            /// </summary>
            private readonly ApplicationContext _context;

            /// <summary>
            /// Маппер для маппинга моделей.
            /// </summary>
            private readonly IMapper _mapper;

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
            public GetDefaultRoleHandler(ApplicationContext context, IMapper mapper, ILogger<GetDefaultRoleHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<RoleModel> Handle(GetDefaultRoleQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    //Получаем роль пользователя по умолчанию при регистрации.
                    var role = await _context.Roles
                        .SingleAsync(role => role.Name == "Пользователь");

                    return _mapper.Map<RoleModel>(role);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Ошибка регистрации");
                }
            }
        }
    }
}
