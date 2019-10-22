using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace NetBaires.Api.Auth
{
    public static class TokenService
    {
        public static string Generate(string secret, List<Claim> claims, DateTime? expire = null)
        {
            if (expire == null)
                expire = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expire.Value,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static ValidateToken Validate(string secret, string tokenToValidate)
        {
            var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(tokenToValidate, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out var rawValidatedToken);
            return new ValidateToken
            {
                Claims = claimsPrincipal.Claims.ToList(),
                Valid = true
            };
        }
    }
}