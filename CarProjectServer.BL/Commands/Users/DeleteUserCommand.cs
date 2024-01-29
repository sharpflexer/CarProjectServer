using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class DeleteUserCommand : IRequest
    {
        public UserModel User { get; set; }

        public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
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
            public DeleteUserHandler(ApplicationContext context, IMapper mapper, ILogger<DeleteUserHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task Handle(DeleteUserCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(u => u.Id == command.User.Id);
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Невозможно удалить пользователя");
                }
            }
        }
    }
}
