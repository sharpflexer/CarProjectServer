using CarProjectServer.BL.Queries.TechnicalWork;
using CarProjectServer.BL.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;
using System.Text;

namespace CarProjectServer.API.HostServices
{
    public class TechnicalWorkHostService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly IWebSocketService _webSocketService;

        private bool WorkWasStarted = false;
        private bool WorkWasStopped = false;

        private const string isAvailableMessage = "AVAILABLE";

        public TechnicalWorkHostService(IServiceScopeFactory serviceScopeFactory,
            IHostApplicationLifetime lifetime,
            IWebSocketService webSocketService)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _lifetime = lifetime;
            _webSocketService = webSocketService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!await WaitForAppStartup(_lifetime, stoppingToken))
                return;

            await DoWorkAsync(stoppingToken);
        }

        private async Task DoWorkAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (IServiceScope scope = _serviceScopeFactory.CreateScope())
                    {
                        ITechnicalWorkService technicalWorkService =
                            scope.ServiceProvider.GetRequiredService<ITechnicalWorkService>();

                        var isTechnicalWork = await technicalWorkService.CheckTechnicalWork();

                        if (isTechnicalWork)
                        {
                            WorkWasStarted = true;
                        }

                        if (!isTechnicalWork)
                        {
                            WorkWasStopped = true;
                        }

                        // Сервер доступен
                        if (WorkWasStarted && WorkWasStopped)
                        {
                            await NotifyIsAvailable();
                            ClearWorkInfo();
                        }

                        if (!WorkWasStarted && WorkWasStopped)
                        {
                            ClearWorkInfo();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
        {
            // 👇 Создаём TaskCompletionSource для ApplicationStarted
            var startedSource = new TaskCompletionSource();
            using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());

            // 👇 Создаём TaskCompletionSource для stoppingToken
            var cancelledSource = new TaskCompletionSource();
            using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

            // Ожидаем любое из событий запуска или запроса на остановку
            Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);

            // Если завершилась задача ApplicationStarted, возвращаем true, иначе false
            return completedTask == startedSource.Task;
        }

        private async Task NotifyIsAvailable()
        {
            await _webSocketService.SendAll(isAvailableMessage);
        }

        private void ClearWorkInfo()
        {
            WorkWasStarted = false;
            WorkWasStopped = false;
        }
    }
}
