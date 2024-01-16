using System.Text.Json.Serialization;

namespace CarProjectServer.API.ViewModels.Google
{
    /// <summary>
    /// Профиль учетной записи Google.
    /// </summary>
    public class GoogleProfileViewModel
    {
        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// E-mail.
        /// </summary>
        [JsonPropertyName("email")]
        public string Email { get; set; }

        /// <summary>
        /// Подтверждена ли почта?
        /// </summary>
        [JsonPropertyName("verified_email")]
        public bool VerifiedEmail { get; set; }

        /// <summary>
        /// Полное имя.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        [JsonPropertyName("family_name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// Фото профиля.
        /// </summary>
        [JsonPropertyName("picture")]
        public string Picture { get; set; }

        /// <summary>
        /// Язык пользователя.
        /// </summary>
        [JsonPropertyName("locale")]
        public string Locale { get; set; }  
    }
}
