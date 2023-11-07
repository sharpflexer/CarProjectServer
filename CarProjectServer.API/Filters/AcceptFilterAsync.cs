using CarProjectServer.BL.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarProjectServer.API.Filters
{
    public class AcceptFilterAsync : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.HttpContext.Request.Headers["Accept"].First() == null)
            {
                throw new ApiException("Некорректный запрос");
            }

            await next();
        }
    }
}
