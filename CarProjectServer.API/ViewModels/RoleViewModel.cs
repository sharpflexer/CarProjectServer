using Microsoft.AspNetCore.Identity;

namespace CarProjectServer.API.ViewModels
{
    public class RoleViewModel : IdentityRole<int>
    {
        /// <summary>
        /// Название роли.
        /// </summary>
        public string Name { get; set; }
    }
}
