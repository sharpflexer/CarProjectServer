using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities;

namespace CarProjectServer.BL.Services.Implementations
{
    public class TechnicalWorksService : ITechnicalWorksService
    {
        private ApplicationContext _context;
        public TechnicalWorksService(ApplicationContext context) 
        {
            _context = context;
        }

        public bool AreTechnicalWorksNow()
        {
            return _context.TechnicalWorks
                .Any(work =>
                work.Start < DateTime.UtcNow &&
                work.End > DateTime.UtcNow);
        }

        public async Task StartWorks(DateTime endTime)
        {
            var technicalWork = new TechnicalWork()
            {
                Start = DateTime.UtcNow,
                End = endTime
            };

            Thread.Sleep(5000);
            _context.TechnicalWorks.Add(technicalWork);
            await _context.SaveChangesAsync();
        }
    }
}
