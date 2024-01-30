using CarProjectServer.BL.Commands.Token;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Extensions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.Token;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Entities.Identity;
using MediatR;
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
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public TokenService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создаёт Access Token.
        /// </summary>
        /// <param name="user">Пользователь для которого создаётся токен.</param>
        /// <returns>Access Token.</returns>
        public async Task<string> CreateAccessToken(UserModel user)
        {
            CreateAccessTokenCommand createAccessToken = new CreateAccessTokenCommand()
            {
                User = user
            };

            return await _mediator.Send(createAccessToken);
        }

        /// <summary>
        /// Создаёт Refresh Token.
        /// </summary>
        /// <returns>Refresh Token.</returns>
        public async Task<string> CreateRefreshToken()
        {
            CreateRefreshTokenCommand createRefreshToken = new CreateRefreshTokenCommand();

            return await _mediator.Send(createRefreshToken);
        }

        /// <summary>
        /// Обновляет устаревший токен.
        /// </summary>
        /// <param name="oldToken">Устаревший токен.</param>
        /// <returns>Новый токен.</returns>
        public async Task<JwtTokenModel> CreateNewToken(JwtTokenModel oldToken)
        {
            CreateNewTokenCommand createNewToken = new CreateNewTokenCommand()
            {
                OldToken = oldToken
            };

            return await _mediator.Send(createNewToken);
        }

        /// <summary>
        /// Получает JWT-токен.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>JWT-токен</returns>
        /// <exception cref="ApiException">Внутренняя ошибка сервера.</exception>
        public async Task<JwtTokenModel> GetJwtToken(string username, string password)
        {
            GetJwtTokenQuery getJwtToken = new GetJwtTokenQuery()
            {
                Username = username,
                Password = password
            };

            return await _mediator.Send(getJwtToken);
        }
    }
}
