using CarProjectServer.BL.Models;
using System.Reflection;

namespace CarProjectServer.BL.Extensions
{
    /// <summary>
    /// Методы расширения для класса User.
    /// </summary>
    public static class UserExtensions
    {
        /// <summary>
        /// Стартовое словое для политик.
        /// </summary>
        private const string StartOfPolicies = "Can";

        /// <summary>
        /// Получает все политики пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns>Список политик.</returns>
        public static IEnumerable<PropertyInfo> GetPolicies(this UserModel user)
        {
            return user.Role
                        .GetType()
                        .GetProperties()
                        .Where(prop => prop.Name.StartsWith(StartOfPolicies));
        }
    }
}
