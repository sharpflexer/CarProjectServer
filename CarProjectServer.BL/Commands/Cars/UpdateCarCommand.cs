using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class UpdateCarCommand : IRequest
    {
        public CarModel Car { get; set; }

        public class UpdateCarHandler : IRequestHandler<UpdateCarCommand>
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
            public UpdateCarHandler(ApplicationContext context, ILogger<UpdateCarHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task Handle(UpdateCarCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var auto = _context.Cars.FirstOrDefault(car => car.Id == command.Car.Id);
                    auto.Brand = _context.Brands.FirstOrDefault(b => b.Id == command.Car.Brand.Id);
                    auto.Model = _context.Models.FirstOrDefault(m => m.Id == command.Car.Model.Id);
                    auto.Color = _context.Colors.FirstOrDefault(c => c.Id == command.Car.Color.Id);
                    _context.Cars.Update(auto);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);

                    throw new ApiException("Невозможно изменить пользователя");
                }
            }
        }
    }
}
