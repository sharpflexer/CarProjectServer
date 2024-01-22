using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Services.Interfaces
{
    public interface ITechnicalWorksService
    {
        public bool AreTechnicalWorksNow();

        public Task StartWorks(DateTime endTime);
    }
}
