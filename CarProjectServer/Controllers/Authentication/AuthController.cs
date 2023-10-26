using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using AutoMapper;

namespace CarProjectServer.API.Controllers.Authentication
{
    /// <summary>
    /// Контроллер для получения и обновления токенов.
    /// </summary>
    public class AuthController : Controller
    {
        /// <summary>
        /// Сервис для работы с JWT токенами.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Маппер для маппинга моделей между слоями
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Инициализирует контроллер сервисами токенов, аутентификации и запросов в БД.
        /// </summary>
        /// <param name="tokenService">Сервис для работы с JWT токенами.</param>
        /// <param name="authenticateService">Сервис для аутентификации пользователей.</param>
        /// <param name="requestService">Сервис для отправки запросов в БД.</param>
        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Проверяет данные пользователя для входа.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>
        /// Результат валидации пользователя.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Token(string username, string password)
        {
            var jwtTokenModel = await _tokenService.GetJwtTokenAsync(username, password);

            JwtTokenViewModel jwtTokenViewModel = _mapper.Map<JwtTokenViewModel>(jwtTokenModel);

            HttpContext.Response.Cookies.Append("Refresh", jwtTokenViewModel.RefreshToken, new CookieOptions()
            {
                HttpOnly = true
            });

            return Ok(jwtTokenViewModel.AccessToken);
        }

        /// <summary>
        /// Обновляет токен.
        /// </summary>
        /// <param name="oldToken">Токен, который требуется заменить.</param>
        /// <returns>Результат обновления.</returns>
        [HttpGet]
        public IActionResult Refresh()
        {
            JwtTokenViewModel oldToken;

            try
            {
                oldToken = TryGetOldJwtToken(HttpContext.Request);
            }
            catch (TokenNotExistException e)
            {
                HttpContext.Response.Headers.Append("TOKEN-NOT-EXIST", "true");
                return BadRequest(e.Message);
            }

            if (oldToken.RefreshToken is null)
            {
                return BadRequest("Invalid client request");
            }

            var newTokenModel = _tokenService.CreateNewToken(_mapper.Map<JwtTokenModel>(oldToken));
            JwtTokenViewModel newToken = _mapper.Map<JwtTokenViewModel>(newTokenModel);

            HttpContext.Response.Cookies.Append("Refresh", newToken.RefreshToken, new CookieOptions()
            {
                HttpOnly = true
            });

            return Ok(newToken.AccessToken);
        }

        /// <summary>
        /// Получает устаревший токен из запроса.
        /// </summary>
        /// <param name="request">HTTP-запрос.</param>
        /// <returns>Устаревший токен.</returns>
        private static JwtTokenViewModel TryGetOldJwtToken(HttpRequest request)
        {
            string oldAccessToken = request.Headers["Authorization"].ToString().Split(" ")[0];

            string? oldRefreshCookie = request.Cookies["Refresh"];
            if (oldRefreshCookie.IsNullOrEmpty())
            {
                throw new TokenNotExistException("Cookies do not contain Refresh Token");
            }

            string oldRefreshToken = oldRefreshCookie.Split(";")[0];

            return new JwtTokenViewModel
            {
                AccessToken = oldAccessToken,
                RefreshToken = oldRefreshToken
            };
        }
    }
}
