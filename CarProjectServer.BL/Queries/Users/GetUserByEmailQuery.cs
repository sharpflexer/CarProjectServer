using AutoMapper;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Context;
using MediatR;
using System.Security.Cryptography;

namespace CarProjectServer.BL.Queries.Users
{
    public class GetUserByEmailQuery : IRequest<UserModel?>
    {
        public string Email { get; set; }

        public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailQuery, UserModel?>
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
            /// Сервис для работы с пользователями.
            /// </summary>
            private readonly IUserService _userService;

            /// <summary>
            /// Инициализирует обработчик контекстом Б Д, маппером и логгером.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            /// <param name="mapper">Маппер для маппинга моделей.</param>
            /// <param name="logger">Логгер для логирования в файлы ошибок. Настраивается в NLog.config.</param>
            public GetUserByEmailHandler(ApplicationContext context, IMapper mapper, IUserService userService)
            {
                _context = context;
                _mapper = mapper;
                _userService = userService;
            }

            public async Task<UserModel?> Handle(GetUserByEmailQuery query, CancellationToken cancellationToken)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == query.Email);

                if (user != null)
                {
                    return _mapper.Map<UserModel?>(user);
                }

                return await GenerateUserByMailAsync(query.Email);
            }


            /// <summary>
            /// Создание пользователя по E-Mail.
            /// </summary>
            /// <param name="email">E-Mail пользователя.</param>
            /// <returns>Пользователь.</returns>
            private async Task<UserModel> GenerateUserByMailAsync(string email)
            {
                string login = GetLoginFromEmail(email);
                string password = GetRandomPassword();

                var user = new UserModel
                {
                    Email = email,
                    Login = login,
                    Password = password
                };

                var roleModel = await _userService.GetDefaultRole();
                user.Role = roleModel;
                await _userService.AddUser(user);

                return user;
            }

            /// <summary>
            /// Генерация случайного пароля.
            /// </summary>
            /// <returns>Случайный пароль.</returns>
            private string GetRandomPassword()
            {
                var randomNumber = new byte[8];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }

            /// <summary>
            /// Создание логина по E-Mail.
            /// </summary>
            /// <param name="email">E-Mail пользователя.</param>
            /// <returns>Логин пользователя.</returns>
            private string GetLoginFromEmail(string email)
            {
                return email.Split('@')[0]; // Никнейм до "@"
            }
        }
    }
}
