namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// JWT токен для аутентификации.
    /// </summary>
    public class JwtTokenModel
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
