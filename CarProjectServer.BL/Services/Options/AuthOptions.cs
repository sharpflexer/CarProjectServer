using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarProjectServer.BL.Services.Options
{
    /// <summary>
    /// Настройки аутентификации JWT токена.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Издатель токена.
        /// </summary>
        public const string Issuer = "http://localhost:5000";

        /// <summary>
        /// Потребитель токена.
        /// </summary>
        public const string Audience = "http://localhost:5001";

        /// <summary>
        /// Ключ для создания токена.
        /// </summary>
        private const string key = "carsupersecret_secretkey!123";

        /// <summary>
        /// </summary>
        /// <returns>Ключ безопасности, который применяется для генерации токена.</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        /// <summary>
        /// Создаёт подпись токена. 
        /// </summary>
        /// <returns>Подпись токена.</returns>
        public static SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
