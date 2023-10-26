using Microsoft.AspNetCore.Identity;

namespace CarProjectServer.DAL.Areas.Identity.Models
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role : IdentityRole<int>
    {
        /// <summary>
        /// Название роли.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Право на создание записей.
        /// </summary>
        public bool CanCreate { get; set; }

        /// <summary>
        /// Право на чтение записей.
        /// </summary
        public bool CanRead { get; set; }

        /// <summary>
        /// Право на обновление записей.
        /// </summary
        public bool CanUpdate { get; set; }

        /// <summary>
        /// Право на удаление записей.
        /// </summary
        public bool CanDelete { get; set; }

        /// <summary>
        /// Право на чтение и изменение данных пользователей.
        /// </summary>
        public bool CanManageUsers { get; set; }
    }
}
