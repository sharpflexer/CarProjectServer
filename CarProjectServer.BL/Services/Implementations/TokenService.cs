using CarProjectServer.BL.Services.Extensions;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.API.Areas.Identity;
using CarProjectServer.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для работы с JWT токенами.
    /// </summary>
    public class TokenService : ITokenService
    {
        /// <summary>
        /// Сервис для отправки запросов в БД.
        /// </summary>
        private readonly IRequestService _requestService;

        /// <summary>
        /// Инициализирует IRequestService
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public TokenService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Создаёт Access Token.
        /// </summary>
        /// <param name="user">Пользователь для которого создаётся токен.</param>
        /// <returns>Access Token.</returns>
        public string CreateToken(User user)
        {
            JwtSecurityToken token = user
                .CreateClaims()
                .CreateJwtToken();
            JwtSecurityTokenHandler tokenHandler = new();

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Создаёт Refresh Token.
        /// </summary>
        /// <returns>Refresh Token.</returns>
        public string CreateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Обновляет устаревший токен.
        /// </summary>
        /// <param name="oldToken">Устаревший токен.</param>
        /// <returns>Новый токен.</returns>
        public JwtToken CreateNewToken(JwtToken oldToken)
        {
            User user = _requestService.GetUserByToken(oldToken.RefreshToken);

            string newAccessToken = "Bearer " + CreateToken(user);
            string newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _ = _requestService.UpdateUser(user);

            return new JwtToken
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
