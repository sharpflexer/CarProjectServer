using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис технических работ.
    /// </summary>
    public class TechnicalWorksService : ITechnicalWorksService
    {
        /// <summary>
        /// Контекст БД.
        /// </summary>
        private ApplicationContext _context;

<<<<<<< HEAD
        private const int timeShift = 5;
        public TechnicalWorksService(ApplicationContext context) 
=======
        /// <summary>
        /// Маппер, для маппинга моделей.
        /// </summary>
        private IMapper _mapper;

        /// <summary>
        /// Задержка начала технических работ.
        /// </summary>
        private const int timeShift = 5;

        /// <summary>
        /// Инициализирует сервис контекстом БД.
        /// </summary>
        /// <param name="context">Контекст БД.</param>
        /// <param name="mapper">Маппер, для маппинга моделей.</param>
        public TechnicalWorksService(ApplicationContext context, IMapper mapper)
>>>>>>> bc61e3c091a1b010ef38bb08e18ee8f5ac70f045
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Проверяет, идут ли сейчас технические работы.
        /// </summary>
        /// <returns>
        /// True - технические работы идут,
        /// False - технических работ сейчас нет.
        /// </returns>
        public bool AreTechnicalWorksNow()
        {
            return _context.TechnicalWorks
                .Any(work =>
                work.Start < DateTime.UtcNow &&
                work.End > DateTime.UtcNow);
        }

        /// <summary>
        /// Начинает технические работы, с заданной задержкой.
        /// </summary>
        /// <param name="endTime">Время окончания технических работ.</param>
        public async Task StartWorks(DateTime endTime)
        {
            var technicalWorkModel = new TechnicalWorkModel()
            {
                Start = DateTime.UtcNow.AddSeconds(timeShift),
                End = endTime.AddSeconds(timeShift)
            };

<<<<<<< HEAD
=======
            var technicalWork = _mapper.Map<TechnicalWork>(technicalWorkModel);
>>>>>>> bc61e3c091a1b010ef38bb08e18ee8f5ac70f045
            _context.TechnicalWorks.Add(technicalWork);
            await _context.SaveChangesAsync();
        }
    }
}
