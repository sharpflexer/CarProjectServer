using CarProjectServer.BL.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace CarProjectServer.API.Options
{
    /// <summary>
    /// Осуществляет настройку авторизации JWT-токена.
    /// Singleton.
    /// </summary>
    public class CarJwtBearerOptions
    {
        private static CarJwtBearerOptions _instance;

        private CarJwtBearerOptions() { }

        private CarJwtBearerOptions(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {

                ValidIssuer = AuthOptions.Issuer,
                ValidAudience = AuthOptions.Audience,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
                    }

                    return Task.CompletedTask;
                }
            };
        }

        public static CarJwtBearerOptions GetInstance(JwtBearerOptions options)
        {
            if (_instance == null)
            {
                _instance = new CarJwtBearerOptions(options);
            }

            return _instance;
        }
    }
}
