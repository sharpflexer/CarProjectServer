using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class DeleteCarCommand : IRequest
    {
        public CarModel Car { get; set; }

        public class DeleteCarHandler : IRequestHandler<DeleteCarCommand>
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
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public DeleteCarHandler(ApplicationContext context, ILogger<DeleteCarHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task Handle(DeleteCarCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var auto = _context.Cars.FirstOrDefault(car => car.Id == command.Car.Id);
                    _context.Cars.Remove(auto);
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
