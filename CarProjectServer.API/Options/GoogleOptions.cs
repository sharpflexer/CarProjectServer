namespace CarProjectServer.API.Options
{
    /// <summary>
    /// Параметры Google API.
    /// </summary>
    public class GoogleOptions
    {
        /// <summary>
        /// ID клиента.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Secret Key клиента.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Адрес, для запроса токена, в обмен на код авторизации.
        /// </summary>
        public string TokenUrl { get; set; }

        /// <summary>
        /// Адрес для запроса E-Mail пользователя.
        /// </summary>
        public string EmailUrl { get; set; }

        /// <summary>
        /// Адрес для перенаправления после авторизации.
        /// </summary>
        public string RedirectUri { get; set; }
    }
}
