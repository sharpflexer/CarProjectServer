using CarProjectServer.API.Models;
using System.Net;

namespace CarProjectServer.API.Middleware
{
    public class CarExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public CarExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
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

    public static class CarExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCarExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CarExceptionMiddleware>();
        }
    }
}
