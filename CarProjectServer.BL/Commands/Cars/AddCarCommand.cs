using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Cars
{
    public class AddCarCommand : IRequest<CarModel>
    {
        public CarModel Car { get; set; }

        public class AddCarHandler : IRequestHandler<AddCarCommand, CarModel>
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
            public AddCarHandler(ApplicationContext context, IMapper mapper, ILogger<AddCarHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<CarModel> Handle(AddCarCommand command, CancellationToken cancellationToken)
            {
                try
                {
                    var auto = _mapper.Map<Car>(command.Car);
                    auto.Brand = _context.Brands.FirstOrDefault(b => b.Id == command.Car.Brand.Id);
                    auto.Model = _context.Models.FirstOrDefault(m => m.Id == command.Car.Model.Id);
                    auto.Color = _context.Colors.FirstOrDefault(c => c.Id == command.Car.Color.Id);
                    var response = _context.Cars.Add(auto);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<CarModel>(response.Entity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    throw new ApiException("Невозможно добавить автомобиль");
                }
            }
        }
    }
}
