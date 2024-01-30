using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Commands.Token
{
    public class StartTechnicalWorkCommand : IRequest
    {
        public DateTime EndTime { get; set; }

        public class StartTechnicalWorkHandler : IRequestHandler<StartTechnicalWorkCommand>
        {
            /// <summary>
            /// Задержка начала технических работ.
            /// </summary>
            private const int timeShift = 5;

            /// <summary>
            /// Контекст для взаимодействия с БД.
            /// </summary>
            private readonly ApplicationContext _context;

            /// <summary>
            /// Маппер для маппинга моделей.
            /// </summary>
            private readonly IMapper _mapper;

            /// <summary>
            /// Инициализирует обработчик контекстом БД и маппером.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            /// <param name="mapper">Маппер для маппинга моделей.</param>
            public StartTechnicalWorkHandler(ApplicationContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task Handle(StartTechnicalWorkCommand command, CancellationToken cancellationToken)
            {
                var technicalWorkModel = new TechnicalWorkModel()
                {
                    Start = DateTime.UtcNow.AddSeconds(timeShift),
                    End = command.EndTime.AddSeconds(timeShift)
                };

                var technicalWork = _mapper.Map<TechnicalWork>(technicalWorkModel);
                _context.TechnicalWorks.Add(technicalWork);

                await _context.SaveChangesAsync();
            }
        }
    }
}
