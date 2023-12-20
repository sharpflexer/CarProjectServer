using Microsoft.AspNetCore.Identity;

namespace CarProjectServer.API.ViewModels
{
    public class RoleViewModel
    {
        /// <summary>
        /// Идентификатор роли.
        /// </summary>
        public int Id { get; set; } 

        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }
    }
}
