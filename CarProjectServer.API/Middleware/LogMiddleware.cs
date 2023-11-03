using Microsoft.AspNetCore.Http;
using System.Text;

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
            httpContext.Request.EnableBuffering();
            var endpoint = httpContext.GetEndpoint()?.DisplayName;
            StreamReader reader = new StreamReader(httpContext.Request.Body);

            try
            {
                if (endpoint != null && endpoint.Contains("API") && !endpoint.Contains("Read"))
                {
                    StringBuilder requestLog = new StringBuilder();
                    requestLog.AppendLine(endpoint);
                    requestLog.AppendLine("METHOD: " + httpContext.Request.Method);
                    requestLog.AppendLine("HEADERS: ");

                    foreach (var key in httpContext.Request.Headers.Keys)
                    {
                        requestLog.AppendLine(key + "=" + httpContext.Request.Headers[key]);
                    }

                    await ReadBody(httpContext, reader, requestLog);

                    _logger.LogInformation(requestLog.ToString());
                }

                await _next(httpContext);
            }
            finally
            {
                reader.Close();
            }
        }

        private async Task ReadBody(HttpContext httpContext, StreamReader reader, StringBuilder requestLog)
        {
            var resultBody = new char[1000];
            await reader.ReadAsync(resultBody, 0, 1000);
            var bodyString = new string(resultBody);
            var formattedBody = bodyString.Replace(Environment.NewLine, string.Empty)
                                          .Replace("\x020", string.Empty)
                                          .Replace("\0", string.Empty);

            requestLog.AppendLine("BODY: ");
            requestLog.Append(formattedBody);

            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
        }
    }
}
