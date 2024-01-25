using CarProjectServer.API.ViewModels;
using System.Text.Json.Serialization;

namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [JsonRequired]
        public int Id { get; set; }

        /// <summary>
        /// Email пользователя, указанный при регистрации.
        /// </summary>
        [JsonRequired]
        public string Email { get; set; }

        /// <summary>
        /// Логин для входа.
        /// </summary>
        [JsonRequired]
        public string Login { get; set; }

        /// <summary>
        /// Пароль для входа.
        /// </summary>
        [JsonRequired]
        public string Password { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary> 
        public RoleViewModel? Role { get; set; }

        /// <summary>
        /// Токен для обновления Access Token.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
