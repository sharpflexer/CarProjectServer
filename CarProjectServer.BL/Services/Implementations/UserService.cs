﻿using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для взаимодействия с пользователями в БД.
    /// </summary>
    public class UserService : IUserService
    {
        /// <summary>
        /// Контекст для взаимодействия с БД.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Маппер для маппинга моделей.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Логгер для логирования в файлы ошибок.
        /// Настраивается в NLog.config.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует сервис контекстом БД и маппером.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        /// <param name="mapper">Маппер для маппинга моделей.</param>
        /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
        public UserService(ApplicationContext context, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        public async Task<RoleModel> GetDefaultRole()
        {
            try
            {
                //Получаем роль пользователя по умолчанию при регистрации.
                var role = await _context.Roles
                    .SingleAsync(role => role.Name == "Пользователь");

                return _mapper.Map<RoleModel>(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка регистрации");
            }
        }

        /// <summary>
        /// Добавляет Refresh Token в таблицу User.
        /// </summary>
        /// <param name="userModel">Аккаунт пользователя.</param>
        /// <param name="refreshToken">Токен для обновления access token.</param>
        public void AddRefreshToken(UserModel userModel, string refreshToken)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userModel.Id);
                user.RefreshToken = refreshToken;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Ошибка аутентификации");
            }
        }

        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        /// <summary>
        public async Task UpdateUser(UserModel userModel)
        {
            try
            {   var user = _context.Users.FirstOrDefault(u => u.Id == userModel.Id);
                var fields = _mapper.Map<User>(userModel);
                var role = _context.Roles.FirstOrDefault(r => r.Id == userModel.Role.Id);

                fields.CopyProperties(user, role);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Невозможно обновить пользователя");
            }
        }

        /// <summary>
        /// Добавляет пользователя в БД при регистрации.
        /// </summary>
        /// <param name="userModel">Аккаунт нового пользователя.</param>
        public async Task AddUserAsync(UserModel userModel)
        {
            try
            {
                var user = _mapper.Map<User>(userModel);
                user.Role = _context.Roles.FirstOrDefault(r => r.Id == user.Role.Id);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Невозможно добавить пользователя");
            }
        }

        /// <summary>
        /// Удаляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для удаления.</param>
        public async Task DeleteUser(UserModel userModel)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == userModel.Id);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Невозможно удалить пользователя");
            }
        }

        /// <summary>
        /// Получает список всех пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            try
            {
                var users = await _context.Users
                    .Include(user => user.Role)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<UserModel>>(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Список пользователей недоступен");
            }
        }

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        public async Task<IEnumerable<RoleModel>> GetRolesAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                var roleModels = _mapper.Map<List<RoleModel>>(roles);

                return roleModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Список ролей недоступен");
            }
        }

        /// <summary>
        /// Ищет пользователя по RefreshToken.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Найденный пользователь.</returns>
        public UserModel GetUserByToken(string refreshToken)
        {
            try
            {
                var user = _context.Users
                    .Include(user => user.Role)
                    .SingleOrDefault(u => u.RefreshToken == refreshToken);
                var userModel = _mapper.Map<UserModel>(user);

                return userModel;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ApiException("Пользователь не найден");
            }
        }

        /// <summary>
        /// Получает наименование роли по имени пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Наименование роли.</returns>
        public async Task<string> GetRoleNameAsync(string username)
        {
            try
            {               
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == username);

                if(user == null)
                {
                    throw new ApiException("Пользователь не найден");
                }
                
                return user.Role.Name;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);

                throw new ApiException("Внутренняя ошибка сервера");
            }
        }
    }
}

