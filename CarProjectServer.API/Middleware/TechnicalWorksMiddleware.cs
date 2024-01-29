using CarProjectServer.API.Models;
using CarProjectServer.BL.Services.Interfaces;

namespace CarProjectServer.API.Middleware
{
    /// <summary>
    /// Middleware для блокирования запросов 
    /// во время технических работ.
    /// </summary>
    public class TechnicalWorksMiddleware
    {
        /// <summary>
        /// Http-запрос.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Инициализирует middleware запросом.
        /// </summary>
        /// <param name="next">Http-запрос.</param>
        public TechnicalWorksMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Обрабатывает запроc и возвращает 503, 
        /// если начались технические работы.
        /// </summary>
        /// <param name="httpContext">Контекст запроса.</param>
        public async Task Invoke(HttpContext httpContext, ITechnicalWorkService worksService)
        {
            if (worksService.IsTechnicalWorkNow())
            {
                httpContext.Response.StatusCode = 503;
                httpContext.Response.ContentType = "application/json";

                var error = new ErrorViewModel
                {
                    StatusCode = httpContext.Response.StatusCode.ToString(),
                    Message = "Технические работы...",
                };

                await httpContext.Response.WriteAsJsonAsync(error);

                return;
            }

            await _next(httpContext);
        }
    }
}
