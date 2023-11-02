using CarProjectServer.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using AutoMapper;
using CarProjectServer.BL.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using CarProjectMVC.Controllers.Authorization;

namespace CarProjectServer.API.Controllers.Authentication
{
    /// <summary>
    /// Контроллер для получения и обновления токенов.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        /// <summary>
        /// Сервис для работы с JWT токенами.
        /// </summary>
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Сервис для аутентификации пользователей.
        /// </summary>
        private readonly IAuthenticateService _authenticateService;

        /// <summary>
        /// Маппер для маппинга моделей между слоями.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует контроллер сервисами токенов, аутентификации и запросов в БД.
        /// </summary>
        /// <param name="tokenService">Сервис для работы с JWT токенами.</param>
        /// <param name="authenticateService">Сервис для аутентификации пользователей.</param>
        /// <param name="mapper">Маппер для маппинга моделей между слоями.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public AuthController(ITokenService tokenService, IAuthenticateService authenticateService, IMapper mapper, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _authenticateService = authenticateService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Проверяет данные пользователя для входа.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>
        /// Результат валидации пользователя.
        /// </returns>
        // POST api/auth/token
        [HttpPost("token")]
        public async Task<ActionResult<string>> Token(string username, string password)
        {
            try
            {
                var jwtTokenModel = await _tokenService.GetJwtTokenAsync(username, password);

                var jwtTokenViewModel = _mapper.Map<JwtTokenViewModel>(jwtTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", jwtTokenViewModel.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true
                });

                return jwtTokenViewModel.AccessToken;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// <summary>
        /// Проверяет данные пользователя для входа.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>
        /// Access-токен
        /// </returns>
        // GET api/auth/refresh
        [HttpGet]
        [HttpGet("refresh")]
        public async Task<ActionResult<string>> Refresh()
        {
            JwtTokenViewModel oldToken;

            try
            {
                oldToken = TryGetOldJwtToken(HttpContext.Request);


                var oldTokenModel = _mapper.Map<JwtTokenModel>(oldToken);
                var newTokenModel = _tokenService.CreateNewToken(oldTokenModel);
                var newToken = _mapper.Map<JwtTokenViewModel>(newTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", newToken.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true
                });

                return Ok(newToken.AccessToken);
            }
            catch (ApiException) 
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// <summary>
        /// Инициализирует контроллер сервисом запросов в БД.
        /// </summary>
        /// <returns>200 OK.</returns>
        // GET api/auth/logout
        [Authorize]
        [HttpGet("logout")]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("Refresh");
                HttpContext.Response.Headers.Remove("Authentication");
                var refreshCookie = HttpContext.Request.Cookies["Refresh"];
                _authenticateService.Revoke(refreshCookie);

                return Ok();
            }
            catch(ApiException ex)
            {
                throw ex;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// <summary>
        /// Получает устаревший токен из запроса.
        /// </summary>
        /// <param name="request">HTTP-запрос.</param>
        /// <returns>Устаревший токен.</returns>
        private JwtTokenViewModel TryGetOldJwtToken(HttpRequest request)
        {
            var oldAccessToken = request.Headers["Authorization"].ToString().Split(" ")[0];

            var oldRefreshCookie = request.Cookies["Refresh"];
            if (oldRefreshCookie.IsNullOrEmpty())
            {
                throw new TokenNotExistException("Cookies do not contain Refresh Token");
            }

            var oldRefreshToken = oldRefreshCookie.Split(";")[0];

            return new JwtTokenViewModel
            {
                AccessToken = oldAccessToken,
                RefreshToken = oldRefreshToken
            };
        }
    }
}
