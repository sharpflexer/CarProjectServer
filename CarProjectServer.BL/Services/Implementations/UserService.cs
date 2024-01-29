using AutoMapper;
using CarProjectServer.BL.Commands.Cars;
using CarProjectServer.BL.Commands.Token;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.Roles;
using CarProjectServer.BL.Queries.Users;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using MediatR;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Claims;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для взаимодействия с пользователями в БД.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Добавляет Refresh Token в таблицу User.
        /// </summary>
        /// <param name="userModel">Аккаунт пользователя.</param>
        /// <param name="refreshToken">Токен для обновления access token.</param>
        public async Task AddRefreshToken(UserModel userModel, string refreshToken)
        {
            AddRefreshTokenCommand addRefreshToken = new AddRefreshTokenCommand()
            {
                User = userModel,
                RefreshToken = refreshToken
            };

            await _mediator.Send(addRefreshToken);
        }

        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        /// <summary>
        public async Task UpdateUser(UserModel userModel)
        {
            UpdateUserCommand updateUser = new UpdateUserCommand() 
            { 
                User = userModel 
            };

            await _mediator.Send(updateUser);
        }

        /// <summary>
        /// Добавляет пользователя в БД при регистрации.
        /// </summary>
        /// <param name="userModel">Аккаунт нового пользователя.</param>
        public async Task AddUser(UserModel userModel)
        {
            AddUserCommand addUser = new AddUserCommand()
            {
                User = userModel
            };

            await _mediator.Send(addUser);
        }

        /// <summary>
        /// Удаляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для удаления.</param>
        public async Task DeleteUser(UserModel userModel)
        {
            DeleteUserCommand deleteUser = new DeleteUserCommand()
            {
                User = userModel
            };

            await _mediator.Send(deleteUser);
        }

        /// <summary>
        /// Получает список всех пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            GetUsersQuery getUsers = new GetUsersQuery();

            return await _mediator.Send(getUsers);
        }

        /// <summary>
        /// Ищет пользователя по RefreshToken.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Найденный пользователь.</returns>
        public async Task<UserModel> GetUserByToken(string refreshToken)
        {
            GetUserByTokenQuery getUserByToken = new GetUserByTokenQuery();

            return await _mediator.Send(getUserByToken);
        }

        /// <summary>
        /// Получение пользователя по E-Mail.
        /// </summary>
        /// <param name="email">E-mail пользователя.</param>
        /// <returns>Пользователь.</returns>
        public async Task<UserModel?> GetUserByEmail(string email)
        {
            GetUserByEmailQuery getUserByEmail = new GetUserByEmailQuery()
            {
                Email = email
            };

            return await _mediator.Send(getUserByEmail);
        }
    }
}

