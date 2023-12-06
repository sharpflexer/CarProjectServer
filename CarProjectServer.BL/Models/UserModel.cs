using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Models
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class UserModel : IdentityUser<int>
    {
        /// <summary>
        /// Email пользователя, указанный при регистрации.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Логин для входа.
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Пароль для входа.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Роль пользователя, дает права на различные действия с таблицей.
        /// </summary> 
        public RoleModel Role { get; set; }

        /// <summary>
        /// Токен для обновления Access Token.
        /// </summary>
        public string? RefreshToken { get; set; }
    }
}
