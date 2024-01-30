using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Entities;
using CarProjectServer.DAL.Migrations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System.Net.WebSockets;
using System.Text;

namespace CarProjectServer.API.HostServices
{
    public class TechnicalWorkHostService : BackgroundService
    {
        private readonly ITechnicalWorkService _technicalWorkService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private bool WorkWasStarted = false;
        private bool WorkWasStopped = false;

        private const string isAvailableMessage = "AVAILABLE";
        public TechnicalWorkHostService(ITechnicalWorkService technicalWorkService, IHttpContextAccessor httpContextAccessor)
        {
            _technicalWorkService = technicalWorkService;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(stoppingToken.IsCancellationRequested)
            {
                var isTechnicalWork = await _technicalWorkService.CheckTechnicalWork();

                if (isTechnicalWork)
                {
                    WorkWasStarted = true;
                }

                if (!isTechnicalWork)
                {
                    WorkWasStopped = true;
                }

                if(WorkWasStarted && WorkWasStopped)
                {
                    NotifyIsAvailable();
                    ClearWorkInfo();
                }
            }
        }

        private async Task NotifyIsAvailable()
        {
            using WebSocket webSocket = await _httpContextAccessor
                .HttpContext
                .WebSockets
                .AcceptWebSocketAsync();

            byte[] bytes = Encoding.UTF8.GetBytes(isAvailableMessage);

            await webSocket.SendAsync(bytes,
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
        }

        private void ClearWorkInfo()
        {
            WorkWasStarted = false;
            WorkWasStopped = false;
        }
    }
}
