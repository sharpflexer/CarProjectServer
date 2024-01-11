using CarProjectServer.BL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using AutoMapper;
using CarProjectServer.BL.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using CarProjectMVC.Controllers.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace CarProjectServer.API.Controllers.Authentication
{
    /// <summary>
    /// Контроллер для получения и обновления токенов.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
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
        /// Сервис для работы с БД пользоватей.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Фабрика, предоставляющая HttpClient
        /// </summary>
        private readonly IHttpClientFactory _httpFactory;

        /// <summary>
        /// Инициализирует контроллер сервисами токенов, аутентификации и запросов в БД.
        /// </summary>
        /// <param name="tokenService">Сервис для работы с JWT токенами.</param>
        /// <param name="authenticateService">Сервис для аутентификации пользователей.</param>
        /// <param name="mapper">Маппер для маппинга моделей между слоями.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public AuthController(ITokenService tokenService,
            IAuthenticateService authenticateService,
            IMapper mapper,
            ILogger<AuthController> logger,
            IUserService userService,
            IHttpClientFactory httpFactory)
        {
            _tokenService = tokenService;
            _authenticateService = authenticateService;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _httpFactory = httpFactory;
        }

        /// <summary>
        /// Проверяет данные пользователя для входа.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>
        /// Результат валидации пользователя.
        /// </returns>
        // POST api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseViewModel>> Login(CredentialsViewModel credentials)
        {
            try
            {
                var jwtTokenModel = await _tokenService.GetJwtTokenAsync(credentials.Username, credentials.Password);
                var jwtTokenViewModel = _mapper.Map<JwtTokenViewModel>(jwtTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", jwtTokenViewModel.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                });

                string roleName = await _userService.GetRoleNameAsync(credentials.Username);

                return new LoginResponseViewModel 
                { 
                    AccessToken =  jwtTokenViewModel.AccessToken, 
                    RoleName = roleName 
                };
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

        [HttpGet("login_via_google")]
        public async Task<ActionResult> LoginViaGoogle(string authCode)
        {
            var client = _httpFactory.CreateClient("Google");

            var body = new Dictionary<string, string>
            {
                { "client_id", "512072756601-r7ibo68bvteters981sgf84cb5vvarer.apps.googleusercontent.com" },
                { "client_secret",  "=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&" },
                { "code", authCode },
                { "access_type:", "offline" },
                { "grant_type", "authorization_code" }
            };

            var response = await client.PostAsync("https://accounts.google.com/o/oauth2/token", new FormUrlEncodedContent(body));
            return Ok(response);
        }

        /// <summary>
        /// Обновляет Access и Refresh токены
        /// </summary>
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
                var newTokenModel = await _tokenService.CreateNewTokenAsync(oldTokenModel);
                var newToken = _mapper.Map<JwtTokenViewModel>(newTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", newToken.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax,

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
        [HttpGet("logout")]
        public async Task<ActionResult> LogOut()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("Refresh");
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
        /// Проверяет роль пользователя из токена доступа.
        /// </summary>
        /// <returns>Роль пользователя.</returns>
        // GET api/auth/get_role
        [Authorize]
        [HttpGet("get_role")]
        public async Task<ActionResult<string>> GetRole()
        {
            try
            {
                var accessToken = HttpContext.Request.Headers["Authorization"][0].Replace("Bearer ", "");
                var token = new JwtSecurityToken(accessToken);
                var claims = token.Claims;

                string role = _userService.GetRoleByClaims(claims);

                return role;

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
