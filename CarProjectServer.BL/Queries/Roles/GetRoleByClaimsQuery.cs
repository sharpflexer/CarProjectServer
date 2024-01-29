using AutoMapper;
using CarProjectServer.BL.Exceptions;
using CarProjectServer.BL.Models;
using CarProjectServer.DAL.Context;
using CarProjectServer.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarProjectServer.BL.Queries.Roles
{
    public class GetRoleByClaimsQuery : IRequest<string>
    {
        public IEnumerable<Claim> Claims { get; set; }

        public class GetRoleByClaimsHandler : IRequestHandler<GetRoleByClaimsQuery, string>
        {
            /// <summary>
            /// Контекст для взаимодействия с БД.
            /// </summary>
            private readonly ApplicationContext _context;


            /// <summary>
            /// Инициализирует обработчик контекстом Б Д, маппером и логгером.
            /// </summary>
            /// <param name="context">Контекст для взаимодействия с БД.</param>
            public GetRoleByClaimsHandler(ApplicationContext context)
            {
                _context = context;
            }

            public async Task<string> Handle(GetRoleByClaimsQuery query, CancellationToken cancellationToken)
            {
                var userId = int.Parse(query.Claims
                    .Single(c => c.Type == JwtRegisteredClaimNames.NameId)
                    .Value);

                return _context.Users.Include(u => u.Role)
                    .Single(user => user.Id == userId)
                    .Role
                    .Name;
            }
        }
    }
}
