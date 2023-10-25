using CarProjectServer.BL.Services.Interfaces;
using CarProjectServer.DAL.Areas.Identity;
using CarProjectServer.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для отправки запросов в БД.
    /// </summary>
    public class RequestService : IRequestService
    {
        /// <summary>
        /// Контекст для взаимодействия с БД.
        /// </summary>
        private readonly ApplicationContext _context;

        /// <summary>
        /// Инициализирует ApplicationContext.
        /// </summary>
        /// <param name="context">Контекст для взаимодействия с БД.</param>
        public RequestService(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавляет Refresh Token в таблицу User.
        /// </summary>
        /// <param name="user">Аккаунт пользователя.</param>
        /// <param name="refreshToken">Токен для обновления access token.</param>
        public void AddRefreshToken(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        /// <summary>
        /// Отправляет запрос на добавление нового автомобиля в БД через ApplicationContext.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task CreateAsync(IFormCollection form)
        {
            Car Auto = new()
            {
                Brand = _context.Brands.Single(brand => brand.Id == int.Parse(form["Brands"])),
                Model = _context.Models.Single(model => model.Id == int.Parse(form["Models"])),
                Color = _context.Colors.Single(color => color.Id == int.Parse(form["Colors"])),
            };
            _context.Cars.Add(Auto);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Удаляет автомобиль из БД.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task DeleteAsync(IFormCollection form)
        {
            Car Auto = new()
            {
                Id = int.Parse(form["IDs"]),
            };
            _context.Cars.Remove(Auto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Ищет пользователя по RefreshToken.
        /// </summary>
        /// <param name="refreshToken">Токен обновления.</param>
        /// <returns>Найденный пользователь.</returns>
        public User GetUserByToken(string refreshToken)
        {
            User? user = _context.Users.Include(user => user.Role).SingleOrDefault(u => u.RefreshToken == refreshToken);

            return user;
        }

        /// <summary>
        /// Получает список всех автомобилей из БД.
        /// </summary>
        /// <returns>Список автомобилей.</returns>
        public List<Car> Read()
        {
            return _context.Cars.Include(car => car.Brand)
               .Include(car => car.Model)
               .Include(car => car.Color)
               .AsNoTracking().OrderBy(car => car.Id).ToList();
        }

        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        public Role GetDefaultRole()
        {
            //Получаем роль пользователя по умолчанию при регистрации.
            return _context.Roles.Single(role => role.Name == "Пользователь");
        }

        /// <summary>
        /// Обновляет данные автомобиля.
        /// </summary>
        /// <param name="form">Форма с данными списков IDs, Brands, Models и Colors.</param>
        public async Task UpdateAsync(IFormCollection form)
        {
            Car Auto = new()
            {
                Id = int.Parse(form["IDs"]),
                Brand = _context.Brands.Single(brand => brand.Id == int.Parse(form["Brands"])),
                Model = _context.Models.Single(model => model.Id == int.Parse(form["Models"])),
                Color = _context.Colors.Single(color => color.Id == int.Parse(form["Colors"])),
            };
            _context.Cars.Update(Auto);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="user">Пользователь для обновления.</param>
        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Добавляет пользователя в БД при регистрации.
        /// </summary>
        /// <param name="user">Аккаунт нового пользователя.</param>
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает список всех пользователей из БД.
        /// </summary>
        /// <returns>Список пользователей.</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(user => user.Role).ToListAsync();
        }

        /// <summary>
        /// Удаляет пользователя из таблицы.
        /// </summary>
        /// <param name="form">Данные пользователя.</param>
        public async Task DeleteUsersAsync(IFormCollection form)
        {
            User user = new()
            {
                Id = int.Parse(form["ID"]),
                Email = form["Email"],
                Login = form["Login"],
                Password = form["Password"],
                Role = _context.Roles.Single(color => color.Id == int.Parse(form["Roles"]))
            };
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновляет пользователя в таблице.
        /// </summary>
        /// <param name="form">Данные пользователя.</param>
        public async Task UpdateUsersAsync(IFormCollection form)
        {
            User user = new()
            {
                Id = int.Parse(form["ID"]),
                Email = form["Email"],
                Login = form["Login"],
                Password = form["Password"],
                Role = _context.Roles.Single(color => color.Id == int.Parse(form["Roles"]))
            };
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            List<Role> roles = await _context.Roles.ToListAsync();

            return roles;
        }
    }
}
