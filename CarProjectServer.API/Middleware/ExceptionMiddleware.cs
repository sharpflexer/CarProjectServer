using CarProjectServer.API.Models;
using CarProjectServer.BL.Exceptions;
using System.Net;

namespace CarProjectServer.API.Middleware
{
    /// <summary>
    /// Middleware обработчик исключений
    /// </summary>
    public class ExceptionMiddleware
    {
        /// <summary>
        /// Http-запрос.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Инициализирует middleware запросом.
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
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
                
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var error = new ErrorViewModel
                {
                    StatusCode = httpContext.Response.StatusCode.ToString(),
                    Message = ex.Message,
                };

                await httpContext.Response.WriteAsJsonAsync(error);
            }         
        }
    }

    /// <summary>
    /// Расширения для ExceptionMiddleware
    /// </summary>
    public static class ExceptionMiddlewareExtensions
    {
        /// <summary>
        /// Метод расширения для Application Builder.
        /// </summary>
        /// <param name="builder">Конфигуратор пайплана.</param>
        /// <returns>Конфигуратор с добавленным ExceptionMiddleware.</returns>
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
