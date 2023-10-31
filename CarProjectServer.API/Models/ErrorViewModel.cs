namespace CarProjectServer.API.Models
{
    /// <summary>
    /// Ошибка.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Код ошибки.
        /// </summary>
        public required string StatusCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public required string Message { get; set; }
    }
}