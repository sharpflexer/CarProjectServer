using CarProjectServer.BL.Enums;
using CarProjectServer.BL.Models;
using CarProjectServer.BL.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace CarProjectServer.BL.Extensions;

/// <summary>
/// Класс с расширениями для JWT токена.
/// </summary>
public static class JwtBearerExtensions
{
    /// <summary>
    /// Создает свойства пользователя.
    /// </summary>
    /// <param name="user">Аккаунт пользователя.</param>
    /// <returns>Свойства пользователя.</returns>
    public static List<Claim> CreateClaims(this UserModel user)
    {
        List<Claim> claims = new()
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, EpochTime.GetIntDate(DateTime.Now.ToUniversalTime()).ToString()),
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(UserPolicies.CanCreate.ToString(), user.Role.CanCreate.ToString()),
            new(UserPolicies.CanRead.ToString(), user.Role.CanRead.ToString()),
            new(UserPolicies.CanUpdate.ToString(), user.Role.CanUpdate.ToString()),
            new(UserPolicies.CanDelete.ToString(), user.Role.CanDelete.ToString()),
            new(UserPolicies.CanManageUsers.ToString(), user.Role.CanManageUsers.ToString())
        };

        return claims;
    }

    /// <summary>
    /// Создает JWT-токен.
    /// </summary>
    /// <param name="claims">Свойства пользователя.</param>
    /// <returns>JWT-токен.</returns>
    public static JwtSecurityToken CreateJwtToken(this IEnumerable<Claim> claims)
    {
        JwtSecurityToken token = new(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            expires: DateTime.Now.ToUniversalTime().AddMinutes(99),
            signingCredentials: AuthOptions.CreateSigningCredentials()
        );

        return token;
    }
}



