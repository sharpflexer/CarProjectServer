namespace CarProjectServer.API.Controllers.Models
{
    /// <summary>
    /// JWT токен для аутентификации.
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Токен для доступа.
        /// </summary>
        public string? AccessToken { get; set; }

        /// <summary>
        /// Токен для обновления.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
