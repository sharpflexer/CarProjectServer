using CarProjectServer.BL.Models;
using CarProjectServer.BL.Options;
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
            new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
            new Claim("CanCreate", user.Role.CanCreate.ToString()),
            new Claim("CanRead", user.Role.CanRead.ToString()),
            new Claim("CanUpdate", user.Role.CanUpdate.ToString()),
            new Claim("CanDelete", user.Role.CanDelete.ToString()),
            new Claim("CanManageUsers", user.Role.CanManageUsers.ToString())
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
            expires: DateTime.Now.AddMinutes(99),
            signingCredentials: AuthOptions.CreateSigningCredentials()
        );
        return token;
    }
}



