using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
