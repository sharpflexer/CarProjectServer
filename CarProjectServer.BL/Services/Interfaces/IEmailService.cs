using CarProjectServer.DAL.Areas.Identity;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для отправки Email-сообщений пользователю.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Отправляет письмо для подтверждения регистрации.
        /// </summary>
        /// <param name="user">Аккаунт нового пользователя.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="message">Сообщение.</param>
        public Task SendEmailAsync(User user, string subject, string message);
    }
}
