namespace CarProjectServer.API.ViewModels
{
    /// <summary>
    /// Данные, возвращаемые после входа.
    /// </summary>
    public class LoginResponseViewModel
    {
        /// <summary>
        /// Токен доступа.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Название роли пользователя.
        /// </summary>
        public string RoleName { get; set; }
    }
}