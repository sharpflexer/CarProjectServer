using CarProjectServer.BL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.API.Models;
using CarProjectServer.BL.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarProjectServer.API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using System.Text.Json;
using CarProjectServer.API.ViewModels.Google;
using Microsoft.Extensions.Options;

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
        /// Сервис для работы с БД пользователей.
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Сервис для работы с БД ролей.
        /// </summary>
        private readonly IRoleService _roleService;

        /// <summary>
        /// Фабрика, предоставляющая HttpClient.
        /// </summary>
        private readonly IHttpClientFactory _httpFactory;

        /// <summary>
        /// Параметры Google OAuth.
        /// </summary>
        private readonly Options.GoogleOptions _googleOptions;

        /// <summary>
        /// Инициализирует контроллер сервисами токенов, аутентификации и запросов в БД.
        /// </summary>
        /// <param name="tokenService">Сервис для работы с JWT токенами.</param>
        /// <param name="authenticateService">Сервис для аутентификации пользователей.</param>
        /// <param name="userService">Сервис для работы с БД пользователей.</param>
        /// <param name="roleService'">Сервис для работы с БД ролей.</param>
        /// <param name="mapper">Маппер для маппинга моделей между слоями.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        /// <param name="httpFactory">Фабрика, предоставляющая HttpClient.</param>
        /// <param name="googleOptions">Параметры Google OAuth.</param>
        public AuthController(ITokenService tokenService,
            IAuthenticateService authenticateService,
            IMapper mapper,
            ILogger<AuthController> logger,
            IUserService userService,
            IHttpClientFactory httpFactory,
            IOptions<Options.GoogleOptions> googleOptions,
            IRoleService roleService)
        {
            _tokenService = tokenService;
            _authenticateService = authenticateService;
            _mapper = mapper;
            _logger = logger;
            _userService = userService;
            _httpFactory = httpFactory;
            _googleOptions = googleOptions.Value;
            _roleService = roleService;
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
                var jwtTokenModel = await _tokenService.GetJwtToken(credentials.Username, credentials.Password);
                var jwtTokenViewModel = _mapper.Map<JwtTokenViewModel>(jwtTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", jwtTokenViewModel.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax
                });

                string roleName = await _roleService.GetRoleName(credentials.Username);

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
        /// <summary>
        /// Осуществляет OAuth 2.0 аутентификацию через Google API
        /// </summary>
        /// <param name="authCode">Код авторизации.</param>
        /// <returns>
        /// Результат валидации пользователя.
        /// </returns>
        /// <exception cref="ApiException">Исключение, отправляемое клиенту.</exception>
        /// GET api/auth/login_via_google
        [HttpGet("login_via_google")]
        public async Task<ActionResult<LoginResponseViewModel>> LoginViaGoogle(string authCode)
        {
            try
            {
                var client = _httpFactory.CreateClient("Google");

                var accessToken = await ExchangeCodeForToken(authCode, client);
                var userMail = await GetUserMail(accessToken, client);

                var userModel = await _userService.GetUserByEmail(userMail);
                var user = _mapper.Map<UserViewModel?>(userModel);

                return await Login(new CredentialsViewModel
                {
                    Username = user.Login,
                    Password = user.Password
                });
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Ошибка регистрации");
            }
        }

        /// <summary>
        /// Получает E-Mail пользователя через Google API.
        /// </summary>
        /// <param name="accessToken">Токен доступа к Google API пользователя.</param>
        /// <param name="client">HTTP-клиент для запросов Google API.</param>
        /// <returns>E-Mail пользователя.</returns>
        /// <exception cref="ApiException">Исключение, отправляемое клиенту.</exception>
        private async Task<string> GetUserMail(string accessToken, HttpClient client)
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            var response = await client.GetAsync(_googleOptions.EmailUrl);
            var content = await response.Content.ReadAsStringAsync();
            var googleProfile = JsonSerializer.Deserialize<GoogleProfileViewModel>(content);
            if (!googleProfile.VerifiedEmail)
            {
                throw new ApiException("Учётная запись Google не подтверждена.");            
            }

            return googleProfile.Email;
        }

        /// <summary>
        /// Обменивает код авторизации на токен доступа к Google API.
        /// </summary>
        /// <param name="authCode">Код авторизации.</param>
        /// <param name="client">HTTP-клиент для запросов Google API.</param>
        /// <returns>Токен доступа к Google API.</returns>
        private async Task<string> ExchangeCodeForToken(string authCode, HttpClient client)
        {
            var body = CreateBody(authCode);

            var response = await client.PostAsync(_googleOptions.TokenUrl, new FormUrlEncodedContent(body));
            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString
            };

            var googleToken = JsonSerializer.Deserialize<GoogleTokenViewModel>(content, options);

            return googleToken.AccessToken;
        }

        /// <summary>
        /// Создает тело запроса для обмена кода авторизации на токен доступа.
        /// </summary>
        /// <param name="authCode">Код авторизации.</param>
        /// <returns>Тело запроса.</returns>
        private Dictionary<string, string> CreateBody(string authCode)
        {
            return new Dictionary<string, string>
                {
                    { "client_id", "512072756601-r7ibo68bvteters981sgf84cb5vvarer.apps.googleusercontent.com" },
                    { "client_secret",  "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" },
                    { "code", authCode },
                    { "access_type", "offline" },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", _googleOptions.RedirectUri },
                    { "scope", "openid profile email" }
                };
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
            try
            {
                var oldToken = TryGetOldJwtToken(HttpContext.Request);
                var oldTokenModel = _mapper.Map<JwtTokenModel>(oldToken);
                var newTokenModel = await _tokenService.CreateNewToken(oldTokenModel);
                var newToken = _mapper.Map<JwtTokenViewModel>(newTokenModel);

                HttpContext.Response.Cookies.Append("Refresh", newToken.RefreshToken, new CookieOptions()
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,

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

                await _authenticateService.RevokeCookie(refreshCookie);

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
                var accessToken = HttpContext.Request.Headers["Authorization"]
                    .FirstOrDefault()
                    .Replace("Bearer ", "");
                var token = new JwtSecurityToken(accessToken);
                var claims = token.Claims;

                string role = await _roleService.GetRoleByClaims(claims);

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
