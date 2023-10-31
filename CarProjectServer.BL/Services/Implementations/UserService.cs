using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NLog;

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

        private readonly ILogger _logger;

        /// <summary>
        /// Инициализирует сервис контекстом БД и маппером.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        /// <param name="mapper">Маппер для маппинга моделей.</param>
        public UserService(ApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка получения роли по умолчанию");
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
                var user = _mapper.Map<User>(userModel);
                user.RefreshToken = refreshToken;
                _context.Users.Update(user);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка добавления Refresh Token в БД");
            }
        }

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        public async Task UpdateUser(UserModel userModel)
        {
            try
            {
                var user = _mapper.Map<User>(userModel);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка обновления пользователя в БД");
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
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка добавления пользователя в БД");
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
                var user = _mapper.Map<User>(userModel);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка удаления пользователя из БД");
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
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка получения списка пользователей");
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
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка получения списка ролей");
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
                _logger.Error(ex.Message);
                throw new ApiException("Ошибка поиска пользователя по Refresh Token");
            }
        }
    }
}

