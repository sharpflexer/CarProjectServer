using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Queries.Cars
{
    public class GetCarPropertiesQuery : IRequest<CarPropertiesModel>
    {
        public class GetCarPropertiesQueryHandler : IRequestHandler<GetCarPropertiesQuery, CarPropertiesModel>
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
            /// Инициализирует обработчик контекстом Б Д, маппером и логгером.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            /// <param name="mapper">Маппер для маппинга моделей.</param>
            public GetCarPropertiesQueryHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<CarPropertiesModel> Handle(GetCarPropertiesQuery query, CancellationToken cancellationToken)
            {
                List<Brand> brands = await _context
                    .Brands
                    .Include(c => c.Models)
                    .AsNoTracking()
                    .ToListAsync();

                List<CarModelType> models = await _context
                    .Models
                    .Include(m => m.Colors)
                    .AsNoTracking()
                    .ToListAsync();

                List<CarColor> colors = await _context
                    .Colors
                    .AsNoTracking()
                    .ToListAsync();

                return new CarPropertiesModel
                {
                    Brands = _mapper.Map<List<BrandModel>>(brands),
                    Models = _mapper.Map<List<CarModelTypeModel>>(models),
                    Colors = _mapper.Map<List<CarColorModel>>(colors)
                };
            }
        }
    }
}
