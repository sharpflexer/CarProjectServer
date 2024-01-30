using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Queries.TechnicalWork
{
    public class CheckTechnicalWorkQuery : IRequest<bool>
    {
        public class CheckTechnicalWorkHandler : IRequestHandler<CheckTechnicalWorkQuery, bool>
        {
            /// <summary>
            /// Контекст для взаимодействия с БД.
            /// </summary>
            private readonly ApplicationContext _context;

            /// <summary>
            /// Инициализирует обработчик контекстом БД.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            public CheckTechnicalWorkHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(CheckTechnicalWorkQuery query, CancellationToken cancellationToken)
            {
                return _context.TechnicalWorks
                    .Any(work =>
                    work.Start < DateTime.UtcNow &&
                    work.End > DateTime.UtcNow);
            }
        }
    }
}
