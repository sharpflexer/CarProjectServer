using CarProjectServer.DAL.Context;
using Microsoft.AspNetCore.Identity;

namespace CarProjectServer.DAL.Entities.Identity
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User : IdentityUser<int>
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
        public Role Role { get; set; }

        /// <summary>
        /// Токен для обновления Access Token.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Копирует свойства данного пользователя в user.
        /// </summary>
        /// <param name="user">Пользователь, в которого копируются свойства</param>
        /// <param name="role">Роль, которая копируется в пользователя</param>
        public void CopyFields(User user, Role role)
        {
            user.Email = Email;
            user.Login = Login;
            user.Password = Password;
            user.PhoneNumber = PhoneNumber;
            user.RefreshToken = RefreshToken;
            user.Role = role;
        }
    }
}
