using CarProjectServer.BL.Services.Interfaces;
using MimeKit;
using MailKit.Net.Smtp;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.BL.Exceptions;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для отправки Email-сообщений пользователю.
    /// </summary>
    public class EmailService : IEmailService
    {
        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует сервис логгером.
        /// </summary>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Отправляет письмо для подтверждения регистрации.
        /// </summary>
        /// <param name="user">Аккаунт нового пользователя.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="message">Сообщение.</param>
        public async Task SendEmailAsync(User user, string subject, string message)
        {
            try
            {
                MimeMessage emailMessage = CreateMessage(user, subject, message);
                SmtpClient client = await CreateClient();
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Некорректный Email");
            }
        }

        /// <summary>
        /// Создаёт клиент для рассылки email-сообщений.
        /// </summary>
        /// <returns>SMTP-Клиент.</returns>
        private async Task<SmtpClient> CreateClient()
        {
            SmtpClient client = new();
            await client.ConnectAsync("smtp.mail.ru", 25, false);
            await client.AuthenticateAsync("car.webapplication@mail.ru", "nifariankek322!");

            return client;
        }

        /// <summary>
        /// Создаёт сообщение для отправки пользователю на email.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="subject">Тема письма.</param>
        /// <param name="message">Текст письма</param>
        /// <returns></returns>
        private MimeMessage CreateMessage(User user, string subject, string message)
        {
            MimeMessage emailMessage = new();

            emailMessage.From.Add(new MailboxAddress("Car WebApplication", "car.webapplication@mail.ru"));
            emailMessage.To.Add(new MailboxAddress(user.Login, user.Email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            return emailMessage;
        }
    }
}
