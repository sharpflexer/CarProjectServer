using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Queries.Users
{
    public class GetUsersQuery : IRequest<IEnumerable<UserModel>>
    {
        public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserModel>>
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
            public GetUsersHandler(ApplicationContext context, IMapper mapper, ILogger<GetUsersHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<IEnumerable<UserModel>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var users = await _context.Users
                        .Include(user => user.Role)
                        .ToListAsync();

                    return _mapper.Map<IEnumerable<UserModel>>(users);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Список пользователей недоступен");
                }
            }
        }
    }
}
