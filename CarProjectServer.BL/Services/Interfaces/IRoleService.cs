using CarProjectServer.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarProjectServer.BL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для получения информации о ролях.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        Task<RoleModel> GetDefaultRole();

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        Task<IEnumerable<RoleModel>> GetRoles();

        /// <summary>
        /// Получает наименование роли по имени пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Наименование роли.</returns>
        Task<string> GetRoleName(string username);

        /// <summary>
        /// Получает роль на основе прав пользователя.
        /// </summary>
        /// <param name="claims">Права пользователя.</param>
        /// <returns>Роль пользователя.</returns>
        Task<string> GetRoleByClaims(IEnumerable<Claim> claims);
    }
}
