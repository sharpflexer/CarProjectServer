using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
using System.Net;

namespace CarProjectServer.API.Middleware
{
    /// <summary>
    /// Middleware обработчик исключений.
    /// </summary>
    public class ExceptionMiddleware
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
        /// <param name="next">Http-запрос.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок.</param>
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            try
            {
                await _next(httpContext);
            }
            catch (ApiException ex)
            {
                await AddExceptionToResponse(httpContext, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                await AddExceptionToResponse(httpContext, "Непредвиденная ошибка взаимодействия с сервером");
            }
        }

        private static async Task AddExceptionToResponse(HttpContext httpContext, string message)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var error = new ErrorViewModel
            {
                StatusCode = httpContext.Response.StatusCode.ToString(),
                Message = message,
            };

            await httpContext.Response.WriteAsJsonAsync(error);
        }
    }
}
