using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;

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
    }
}
