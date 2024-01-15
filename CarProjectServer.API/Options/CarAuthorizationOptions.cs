using Microsoft.AspNetCore.Authorization;

namespace CarProjectServer.API.Options
{
    /// <summary>
    /// Осуществляет настройку политик авторизации.
    /// Singleton.
    /// </summary>
    public class CarAuthorizationOptions : AuthorizationOptions
    {
        private static CarAuthorizationOptions _instance;

        private CarAuthorizationOptions() { }

        private CarAuthorizationOptions(AuthorizationOptions options) 
        {
            options.AddPolicy("Create", policy =>
            {
                policy.RequireClaim("CanCreate", "True");
            });
            options.AddPolicy("Read", policy =>
            {
                policy.RequireClaim("CanRead", "True");
            });
            options.AddPolicy("Update", policy =>
            {
                policy.RequireClaim("CanUpdate", "True");
            });
            options.AddPolicy("Delete", policy =>
            {
                policy.RequireClaim("CanDelete", "True");
            });
            options.AddPolicy("ReadProperties", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim("CanCreate", "True") ||
                    context.User.HasClaim("CanUpdate", "True")
                )
            );
            options.AddPolicy("Users", policy =>
            {
                policy.RequireClaim("CanManageUsers", "True");
            });
        }

        public static CarAuthorizationOptions GetInstance(AuthorizationOptions options)
        {
            if (_instance == null)
            {
                _instance = new CarAuthorizationOptions(options);
            }

            return _instance;
        }
    }
}
