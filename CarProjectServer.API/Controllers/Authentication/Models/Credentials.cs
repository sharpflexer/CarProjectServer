namespace CarProjectServer.API.Controllers.Authentication.Models
{
    /// <summary>
    /// Данные для авторизации.
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// Логин, имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
