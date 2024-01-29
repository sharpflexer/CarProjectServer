using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Queries.Cars
{
    public class GetCarsQuery : IRequest<IEnumerable<CarModel>>
    {
        public class GetCarsQueryHandler : IRequestHandler<GetCarsQuery, IEnumerable<CarModel>>
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
            public GetCarsQueryHandler(ApplicationContext context, IMapper mapper, ILogger<GetCarsQueryHandler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<IEnumerable<CarModel>> Handle(GetCarsQuery query, CancellationToken cancellationToken)
            {
                try
                {
                    var cars = await _context.Cars
                       .Include(car => car.Brand)
                       .Include(car => car.Model)
                       .Include(car => car.Color)
                       .AsNoTracking()
                       .OrderBy(car => car.Id)
                       .ToListAsync();

                    return _mapper.Map<List<CarModel>>(cars);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);

                    throw new ApiException("Список автомобилей недоступен");
                }
            }
        }
    }
}
