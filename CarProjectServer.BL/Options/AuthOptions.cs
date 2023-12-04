using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarProjectServer.BL.Options
{
    /// <summary>
    /// Настройки аутентификации JWT токена.
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Издатель токена.
        /// </summary>
        public const string Issuer = "CarProjectServer";

        /// <summary>
        /// Потребитель токена.
        /// </summary>
        public const string Audience = "CarProjectClient";

        /// <summary>
        /// Ключ для создания токена.
        /// </summary>
        private const string key = "carsupersecret_secretkey!123_VERY_BIG_KEY_FOR_SASHA_256";

        /// <summary>
        /// Получает ключ безопасности
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
