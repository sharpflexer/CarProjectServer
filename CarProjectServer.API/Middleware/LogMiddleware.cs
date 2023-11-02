using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CarProjectServer.API.Middleware
{
    /// <summary>
    /// Middleware для логирования запросов
    /// </summary>
    public class LogMiddleware
    {
        /// <summary>
        /// Http-запрос.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует middleware запросом и логгером.
        /// </summary>
        /// <param name="next"></param>
        public LogMiddleware(RequestDelegate next, ILogger<LogMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Обрабатывает запрос и исключения.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task Invoke(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint()?.DisplayName;
            if (endpoint != null && endpoint.Contains("API"))
            {
                StringBuilder requestLog = new StringBuilder();
                requestLog.AppendLine(endpoint);
                requestLog.AppendLine("METHOD: " + httpContext.Request.Method);
                requestLog.AppendLine("HEADERS: ");

                foreach (var key in httpContext.Request.Headers.Keys)
                    requestLog.AppendLine(key + "=" + httpContext.Request.Headers[key]);

                StreamReader reader = new StreamReader(httpContext.Request.Body);
                var resultBody = new char[1000];
                await reader.ReadAsync(resultBody, 0, 1000);
                var bodyString = new string(resultBody);
                var formattedBody = bodyString.Replace("\n", "").Replace("\x020", "");

                requestLog.AppendLine("BODY: ");
                requestLog.Append(formattedBody);

                _logger.LogInformation(requestLog.ToString());
            }

            await _next(httpContext);
        }
    }
}
