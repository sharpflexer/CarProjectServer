using AutoMapper;
using CarProjectServer.BL.Commands.Token;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.TechnicalWork;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities;
using MediatR;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис технических работ.
    /// </summary>
    public class TechnicalWorkService : ITechnicalWorkService
    {
        /// <summary>
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public TechnicalWorkService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Проверяет, идут ли сейчас технические работы.
        /// </summary>
        /// <returns>
        /// True - технические работы идут,
        /// False - технических работ сейчас нет.
        /// </returns>
        public async Task<bool> CheckTechnicalWork()
        {
            CheckTechnicalWorkQuery checkTechnicalWork = new CheckTechnicalWorkQuery();

            return await _mediator.Send(checkTechnicalWork);
        }

        /// <summary>
        /// Начинает технические работы, с заданной задержкой.
        /// </summary>
        /// <param name="endTime">Время окончания технических работ.</param>
        public async Task StartTechnicalWork(DateTime endTime)
        {
            StartTechnicalWorkCommand startTechnicalWork = new StartTechnicalWorkCommand()
            {
                EndTime = endTime
            };

            await _mediator.Send(startTechnicalWork);
        }
    }
}
