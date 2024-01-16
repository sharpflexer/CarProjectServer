using System.Text.Json.Serialization;

namespace CarProjectServer.API.ViewModels.Google
{
    /// <summary>
    /// Токен Google API.
    /// </summary>
    public class GoogleTokenViewModel
    {
        /// <summary>
        /// Токен доступа.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Время истечения токена.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Токен обновления.
        /// </summary>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Область доступа.
        /// </summary>
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// Тип токена.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}
