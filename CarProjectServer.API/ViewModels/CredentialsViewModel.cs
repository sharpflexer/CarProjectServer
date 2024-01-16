namespace CarProjectServer.API.ViewModels
{
    /// <summary>
    /// Данные для авторизации.
    /// </summary>
    public class CredentialsViewModel
    {
        /// <summary>
        /// Логин, имя пользователя.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
