using CarProjectServer.API.Timers;
using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace CarProjectServer.API.Controllers.Notification
{
    /// <summary>
    /// Контроллер веб-сокета оповещений.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private delegate Task MessageHandler(WebSocket webSocket);
        private static event MessageHandler NotifyByAdmin;
        private readonly ITechnicalWorksService _technicalWorksService;
        private const string notifyString = "Технические работы!";

        /// <summary>
        /// Фабрика, предоставляющая HttpClient.
        /// </summary>
        private readonly IHttpClientFactory _httpFactory;

        public NotificationController(ITechnicalWorksService technicalWorksService, IHttpClientFactory httpFactory)
        {
            _technicalWorksService = technicalWorksService;
            _httpFactory = httpFactory;
        }

        /// <summary>
        /// Подключение по веб-сокету
        /// </summary>
        /// GET api/notification/ws
        [HttpGet("ws")]
        public async Task Get()
        {
            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            NotificationTimer.GetInstance().Notify += MessageHandler;
            NotifyByAdmin += AdminMessageHandler;

            await Echo(webSocket);

            async Task MessageHandler(string message)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                await webSocket.SendAsync(bytes,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);

                Thread.Sleep(5000);
                await _technicalWorksService.StartWorks(DateTime.UtcNow.AddMinutes(1));
            }

            async Task AdminMessageHandler(WebSocket sender)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(notifyString);

                WebSocket reciever = webSocket;
                if (reciever != sender)
                {
                    await webSocket.SendAsync(
                        bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        [HttpPost("start")]
        public async Task Start([FromBody] string endTime)
        {
            var end = DateTime.Parse(endTime).ToUniversalTime();
            await _technicalWorksService.StartWorks(end);
        }

        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                var client = _httpFactory.CreateClient("Role");
                var accessToken = Request.Headers["Authorization"].ToString();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                var response = await client.GetAsync(client.BaseAddress);
                var role = await response.Content.ReadAsStringAsync();

                if (role == "Админ")
                {
                    await NotifyByAdmin.Invoke(webSocket);

                    receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
                }
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
