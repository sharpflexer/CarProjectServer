using CarProjectServer.DAL.Entities.Identity;
using CarProjectServer.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarProjectServer.DAL.Context
{
    /// <summary>
    /// Контекст для взаимодействия с БД.
    /// </summary>
    public class ApplicationContext : IdentityDbContext<User, Role, int>
    {
        /// <summary>
        /// Таблица автомобилей.
        /// </summary>
        public DbSet<Car> Cars => Set<Car>();

        /// <summary>
        /// Таблица автомобильных марок.
        /// </summary>
        public DbSet<Brand> Brands => Set<Brand>();

        /// <summary>
        /// Таблица моделей автомобилей.
        /// </summary>
        public DbSet<CarModelType> Models => Set<CarModelType>();

        /// <summary>
        /// Таблица расцветок автомобилей.
        /// </summary>
        public DbSet<CarColor> Colors => Set<CarColor>();

        /// <summary>
        /// Таблица ролей.
        /// </summary>
        public DbSet<Role> Roles => Set<Role>();

        /// <summary>
        /// Таблица пользователей.
        /// </summary>
        public DbSet<User> Users => Set<User>();

        /// <summary>
        /// Инициализирует контекст настройками. 
        /// Создаёт базу данных при её отсутствии
        /// и, в случае создания, заполняет данными.
        /// </summary>
        /// <param name="options">Настройки контекста.</param>
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            if (Database.EnsureCreated())
            {
                FillDatabase();
            }
        }

        /// <summary>
        /// Настраивает и инициализирует данными БД при ее создании.
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            AssignAspNetTables(builder);
            IgnoreUselessTables(builder);
        }

        /// <summary>
        /// Привязывает сущности к уже существующим таблицам, 
        /// вместо автоматически созданных "AspNet..." таблиц.
        /// </summary>
        /// <param name="builder">Конструктор БД.</param>
        private void AssignAspNetTables(ModelBuilder builder)
        {
            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
        }

        /// <summary>
        /// Игнорирует ненужные таблицы при создании БД.
        /// </summary>
        /// <param name="builder">Конструктор БД.</param>
        private void IgnoreUselessTables(ModelBuilder builder)
        {
            builder.Ignore<IdentityUserClaim<int>>();
            builder.Ignore<IdentityUserToken<int>>();
            builder.Ignore<IdentityUserClaim<int>>();
            builder.Ignore<IdentityUserLogin<int>>();
            builder.Ignore<IdentityRoleClaim<int>>();
            builder.Ignore<IdentityUserRole<int>>();
            builder.Ignore<IdentityRole<int>>();
        }

        private void FillDatabase()
        {
            Colors.AddRange(
                new CarColor { Name = "Red" },
                new CarColor { Name = "Blue" },
                new CarColor { Name = "Green" },
                new CarColor { Name = "Yellow" },
                new CarColor { Name = "Gray" },
                new CarColor { Name = "Black" },
                new CarColor { Name = "Dark Blue" },
                new CarColor { Name = "White" });
            SaveChanges();
            Models.AddRange(
                new CarModelType()
                {
                    Name = "A3",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(1),
                        Colors.ToList().ElementAt(4),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "A5",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(2),
                        Colors.ToList().ElementAt(4),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "A6",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(3),
                        Colors.ToList().ElementAt(4),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "X3",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(3),
                        Colors.ToList().ElementAt(1),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "X5",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(3),
                        Colors.ToList().ElementAt(2),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "X6",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(3),
                        Colors.ToList().ElementAt(6),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "GLE",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(1),
                        Colors.ToList().ElementAt(6),
                        Colors.ToList().ElementAt(2)
                    }
                },
                new CarModelType()
                {
                    Name = "GLB",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(1),
                        Colors.ToList().ElementAt(4),
                        Colors.ToList().ElementAt(5)
                    }
                },
                new CarModelType()
                {
                    Name = "GLC",
                    Colors = new List<CarColor>() {
                        Colors.ToList().ElementAt(1),
                        Colors.ToList().ElementAt(2),
                        Colors.ToList().ElementAt(3)
                    }
                }
            );
            SaveChanges();
            foreach (CarColor color in Colors.ToList())
            {
                color.Models = Models.Where(model => model.Colors.Contains(color)).ToList();
            }
            SaveChanges();
            Brands.AddRange(
                new Brand()
                {
                    Name = "Audi",
                    Models = Models.ToList().Where(m => m.Name.Contains('A')).ToList(),
                },
                new Brand()
                {
                    Name = "BMW",
                    Models = Models.ToList().Where(m => m.Name.Contains('X')).ToList(),
                },
                new Brand()
                {
                    Name = "Mercedes-Benz",
                    Models = Models.ToList().Where(m => m.Name.Contains("GL")).ToList(),
                }
            );
            SaveChanges();
            Roles.AddRange(
                new Role()
                {
                    Name = "Админ",
                    CanCreate = true,
                    CanRead = true,
                    CanUpdate = true,
                    CanDelete = true,
                    CanManageUsers = true
                },
                new Role()
                {
                    Name = "Менеджер",
                    CanCreate = true,
                    CanRead = true,
                    CanUpdate = true,
                    CanDelete = true,
                    CanManageUsers = false
                },
                new Role()
                {
                    Name = "Пользователь",
                    CanCreate = false,
                    CanRead = true,
                    CanUpdate = false,
                    CanDelete = false,
                    CanManageUsers = false
                });
            SaveChanges();
            Users.AddRange(
                new User() { Email = "admin456@mail.ru", Login = "admin", Password = "admin123", Role = Roles.Single(role => role.Name == "Админ") },
                new User() { Email = "manager456@gmail.com", Login = "manager", Password = "manager123", Role = Roles.Single(role => role.Name == "Менеджер") },
                new User() { Email = "user456@yandex.ru", Login = "user", Password = "user123", Role = Roles.Single(role => role.Name == "Пользователь") }
            );
            SaveChanges();
        }
    }
}
