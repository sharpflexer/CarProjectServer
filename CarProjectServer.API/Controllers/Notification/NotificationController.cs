using CarProjectServer.API.Timers;
using CarProjectServer.BL.Services.Interfaces;
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
        /// <summary>
        /// Делегат для обработчика оповещения админа.
        /// </summary>
        /// <param name="webSocket">Веб-сокет, который отправил соообщение.</param>
        private delegate Task MessageHandler(WebSocket webSocket);

        /// <summary>
        /// Событие для отправки сообщений от администратора о технических работах.
        /// </summary>
        private static event MessageHandler NotifyByAdmin;

        /// <summary>
        /// Сервис технических работ.
        /// </summary>
        private readonly ITechnicalWorksService _technicalWorksService;

        /// <summary>
        /// Сообщение о технических работах.
        /// </summary>
        private const string notifyString = "Технические работы!";

        /// <summary>
        /// Фабрика, предоставляющая HttpClient.
        /// </summary>
        private readonly IHttpClientFactory _httpFactory;

        /// <summary>
        /// Инициализирует контроллер сервисом для технических работ и фабрикой Http-клиентов.
        /// </summary>
        /// <param name="technicalWorksService">Сервис технических работ.</param>
        /// <param name="httpFactory">Фабрика, предоставляющая HttpClient.</param>
        public NotificationController(ITechnicalWorksService technicalWorksService, IHttpClientFactory httpFactory)
        {
            _technicalWorksService = technicalWorksService;
            _httpFactory = httpFactory;
        }

        /// <summary>
        /// Подключение по веб-сокету.
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

                await _technicalWorksService.StartWorks(DateTime.UtcNow.AddMinutes(1));
            }

            async Task AdminMessageHandler(WebSocket sender)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(notifyString);

                WebSocket reciever = webSocket;
                if (reciever != sender)
                {
                    await webSocket.SendAsync(bytes, 
                        WebSocketMessageType.Text, 
                        true, 
                        CancellationToken.None);
                }
            }
        }

        /// <summary>
        /// Начинает технические работы.
        /// </summary>
        /// <param name="endTime">Время окончания технических работ.</param>
        [HttpPost("start")]
        public async Task Start([FromBody] string endTime)
        {
            var end = DateTime.Parse(endTime).ToUniversalTime();
            await _technicalWorksService.StartWorks(end);
        }

        /// <summary>
        /// Обрабатывает сообщения по веб-сокетам.
        /// </summary>
        /// <param name="webSocket">Веб-сокет, по которому обрабатываются сообщения.</param>
        private async Task Echo(WebSocket webSocket)
        {
            byte[] buffer = new byte[1024 * 4];
            WebSocketReceiveResult receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            var client = _httpFactory.CreateClient("Role");
            receiveResult = await HandleMessages(webSocket, buffer, receiveResult, client);

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        /// <summary>
        /// Обрабатывает сообщения, пока не закроется сокет.
        /// </summary>
        /// <param name="webSocket">Веб-сокет для обработки сообщений.</param>
        /// <param name="buffer">Сообщение в виде массива байтов.</param>
        /// <param name="receiveResult">Результат получения сообщения.</param>
        /// <param name="client">Http-клиент для проверки роли.</param>
        private static async Task<WebSocketReceiveResult> HandleMessages(WebSocket webSocket, byte[] buffer, WebSocketReceiveResult receiveResult, HttpClient client)
        {
            var message = Encoding.UTF8.GetString(buffer);

            if (message.Contains("Authorization"))
            {
                string role = await GetRoleByMessage(client, message);

                if (role == "Админ")
                {
                    await NotifyByAdmin.Invoke(webSocket);
                }
            }

            receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None); // Получение сообщения по веб-сокету.

            if (!receiveResult.CloseStatus.HasValue) // Проверка на закрытие сокета.
            {
                return await HandleMessages(webSocket, buffer, receiveResult, client);
            }

            return receiveResult;
        }

        /// <summary>
        /// Получает роль пользователя из токена в сообщении.
        /// </summary>
        /// <param name="client">Http-клиент для проверки роли.</param>
        /// <param name="message">Сообщение, содержащее токен доступа.</param>
        /// <returns>Роль пользователя.</returns>
        private static async Task<string> GetRoleByMessage(HttpClient client, string message)
        {
            var accessToken = message.Split(": ").Last().Replace("\0", "");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await client.GetAsync(client.BaseAddress);
            var role = await response.Content.ReadAsStringAsync();

            return role;
        }
    }
}
