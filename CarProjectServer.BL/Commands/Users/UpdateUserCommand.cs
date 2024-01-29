using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class UpdateUserCommand : IRequest
    {
        public UserModel User { get; set; }

        public class UpdateUserdHandler : IRequestHandler<UpdateUserCommand>
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
            public UpdateUserdHandler(ApplicationContext context, IMapper mapper, ILogger<UpdateUserdHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task Handle(UpdateUserCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.Id == command.User.Id);
                    var fields = _mapper.Map<User>(command.User);
                    var role = _context.Roles.FirstOrDefault(r => r.Id == command.User.Role.Id);

                    fields.CopyProperties(user, role);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Невозможно обновить пользователя");
                }
            }
        }
    }
}
