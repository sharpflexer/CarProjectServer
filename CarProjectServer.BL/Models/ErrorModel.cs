namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Ошибка.
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// Идентификатор запроса.
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Статус отображения идентификатора запроса.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}