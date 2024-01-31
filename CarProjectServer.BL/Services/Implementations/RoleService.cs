using CarProjectServer.BL.Models;
using CarProjectServer.BL.Queries.Roles;
using CarProjectServer.BL.Services.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarProjectServer.BL.Services.Implementations
{
    /// <summary>
    /// Сервис для получения информации о ролях.
    /// </summary>
    public class RoleService : IRoleService
    {
        /// <summary>
        /// Посредник.
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Инициализирует сервис посредником.
        /// </summary>
        /// <param name="mediator">Посредник.</param>
        public RoleService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получает роль пользователя по умолчанию(при регистрации).
        /// </summary>
        /// <returns>Роль по умолчанию</returns>
        public async Task<RoleModel> GetDefaultRole()
        {
            GetDefaultRoleQuery getDefaultRole = new GetDefaultRoleQuery();

            return await _mediator.Send(getDefaultRole);
        }

        /// <summary>
        /// Получает список всех возможных ролей пользователей.
        /// </summary>
        /// <returns>Список всех ролей.</returns>
        public async Task<IEnumerable<RoleModel>> GetRoles()
        {
            GetRolesQuery getRoles = new GetRolesQuery();

            return await _mediator.Send(getRoles);
        }

        /// <summary>
        /// Получает наименование роли по имени пользователя.
        /// </summary>
        /// <param name="username">Имя пользователя.</param>
        /// <returns>Наименование роли.</returns>
        public async Task<string> GetRoleName(string username)
        {
            GetRoleNameQuery getUserByToken = new GetRoleNameQuery()
            {
                Username = username
            };

            return await _mediator.Send(getUserByToken);
        }

        /// <summary>
        /// Получает роль на основе прав пользователя.
        /// </summary>
        /// <param name="claims">Права пользователя.</param>
        /// <returns>Роль пользователя.</returns>
        public async Task<string> GetRoleByClaims(IEnumerable<Claim> claims)
        {
            GetRoleByClaimsQuery getRoleByClaims = new GetRoleByClaimsQuery()
            {
                Claims = claims
            };

            return await _mediator.Send(getRoleByClaims);
        }
    }
}
