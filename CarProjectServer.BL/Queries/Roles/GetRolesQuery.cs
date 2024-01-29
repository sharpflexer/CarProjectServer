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
    public class GetRolesQuery : IRequest<IEnumerable<RoleModel>>
    {
        public class GetRolesHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleModel>>
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
            public GetRolesHandler(ApplicationContext context, IMapper mapper, ILogger<GetRolesHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<IEnumerable<RoleModel>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var roles = await _context.Roles.ToListAsync();
                    var roleModels = _mapper.Map<List<RoleModel>>(roles);

                    return roleModels;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Список ролей недоступен");
                }
            }
        }
    }
}
