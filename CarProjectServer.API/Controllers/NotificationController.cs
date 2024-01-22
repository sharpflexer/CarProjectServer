using CarProjectServer.BL.Services.Implementations;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace CarProjectServer.API.Controllers
{
    /// <summary>
    /// Контроллер веб-сокета оповещений.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private delegate Task MessageHandler(byte[] message, WebSocket webSocket);
        private static event MessageHandler NotifyByAdmin;
        private readonly ITechnicalWorksService _technicalWorksService;

        public NotificationController(ITechnicalWorksService technicalWorksService)
        {
            _technicalWorksService = technicalWorksService;
        }


        /// <summary>
        /// Подключение по веб-сокету
        /// </summary>
        /// GET api/notification/ws
        [HttpGet("ws")]
        public async Task Get()
        {
            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            NotificationTimer.Notify += MessageHandler;

            await Echo(webSocket);

            async Task MessageHandler(string message)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(message);

                await webSocket.SendAsync(
                    bytes, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        [HttpPost("start")]
        public async Task Start([FromBody] string endTime)
        {
            var end = DateTime.Parse(endTime).ToUniversalTime();
            await _technicalWorksService.StartWorks(end);          
        }

        private async Task NotifyAllAsync()
        {
            using WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            NotifyByAdmin += MessageHandler;

            await Echo(webSocket);

            async Task MessageHandler(byte[] message, WebSocket sender)
            {
                WebSocket reciever = webSocket;
                if (reciever != sender)
                {
                    await webSocket.SendAsync(
                        message, WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }

        private static async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                NotifyByAdmin.Invoke(buffer, webSocket);

                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}
