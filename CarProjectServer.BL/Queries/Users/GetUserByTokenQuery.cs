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
    public class GetUserByTokenQuery : IRequest<UserModel>
    {
        public string RefreshToken { get; set; }

        public class GetUserByTokenHandler : IRequestHandler<GetUserByTokenQuery, UserModel>
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
            public GetUserByTokenHandler(ApplicationContext context, IMapper mapper, ILogger<GetUserByTokenHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<UserModel> Handle(GetUserByTokenQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var user = _context.Users
                        .Include(user => user.Role)
                        .SingleOrDefault(u => u.RefreshToken == query.RefreshToken);
                    var userModel = _mapper.Map<UserModel>(user);

                    return userModel;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Пользователь иди нахуй");
                }
            }
        }
    }
}
