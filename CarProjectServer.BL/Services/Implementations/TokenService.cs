using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Extensions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using Microsoft.Extensions.Logging;
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
        /// Сервис для взаимодействия с пользователями в БД.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует сервис сервисом пользователей, аутентификации и логгером.
        /// </summary>
        /// <param name="userService">Сервис для взаимодействия с пользователями в БД.</param>
        /// <param name="authenticateService">Сервис для аутентификации пользователей</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public TokenService(IUserService userService, IAuthenticateService authenticateService, ILogger<TokenService> logger)
        {
            _userService = userService;
            _authenticateService = authenticateService;
            _logger = logger;
        }

        /// <summary>
        /// Создаёт Access Token.
        /// </summary>
        /// <param name="user">Пользователь для которого создаётся токен.</param>
        /// <returns>Access Token.</returns>
        public string CreateToken(UserModel user)
        {
            try
            {
                var token = user.CreateClaims()
                    .CreateJwtToken();
                JwtSecurityTokenHandler tokenHandler = new();

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// <summary>
        /// Создаёт Refresh Token.
        /// </summary>
        /// <returns>Refresh Token.</returns>
        public string CreateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// <summary>
        /// Обновляет устаревший токен.
        /// </summary>
        /// <param name="oldToken">Устаревший токен.</param>
        /// <returns>Новый токен.</returns>
        public async Task<JwtTokenModel> CreateNewTokenAsync(JwtTokenModel oldToken)
        {
            try
            {
                var user = _userService.GetUserByToken(oldToken.RefreshToken);

                var newAccessToken = "Bearer " + CreateToken(user);
                var newRefreshToken = CreateRefreshToken();

                _userService.AddRefreshToken(user, newRefreshToken);

                return new JwtTokenModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        public async Task<JwtTokenModel> GetJwtTokenAsync(string username, string password)
        {
            try
            {
                var user = await _authenticateService.AuthenticateUser(username, password);
                var accessToken = CreateToken(user);
                var refreshToken = CreateRefreshToken();

                _userService.AddRefreshToken(user, refreshToken);

                return new JwtTokenModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }
    }
}
