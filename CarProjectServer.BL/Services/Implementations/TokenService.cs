using CarProjectServer.BL.Extensions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
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
        /// Сервис для взаимодействия с автомобилями в БД.
        /// </summary>
        private readonly ICarService _carService;

        /// <summary>
        /// Сервис для взаимодействия с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        /// <summary>
        /// Инициализирует IRequestService
        /// </summary>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public TokenService(ICarService requestService)
        {
            _carService = requestService;
        }

        /// <summary>
        /// Создаёт Access Token.
        /// </summary>
        /// <param name="user">Пользователь для которого создаётся токен.</param>
        /// <returns>Access Token.</returns>
        public string CreateToken(UserModel user)
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
            var randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Обновляет устаревший токен.
        /// </summary>
        /// <param name="oldToken">Устаревший токен.</param>
        /// <returns>Новый токен.</returns>
        public JwtTokenModel CreateNewToken(JwtTokenModel oldToken)
        {
            var user = _userService.GetUserByToken(oldToken.RefreshToken);

            var newAccessToken = "Bearer " + CreateToken(user);
            var newRefreshToken = CreateRefreshToken();

            user.RefreshToken = newRefreshToken;
            _ = _userService.UpdateUser(user);

            return new JwtTokenModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<JwtTokenModel> GetJwtTokenAsync(string username, string password)
        {
            var user = await _authenticateService.AuthenticateUser(username, password);
            var accessToken = CreateToken(user);
            var refreshToken = CreateRefreshToken();

            return new JwtTokenModel 
            { 
                AccessToken = accessToken, 
                RefreshToken = refreshToken 
            };
        }
    }
}
