using Core;
using Core.Interfeses;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Aplication
{
    public class JwtProvider: IJwtProvider
    {
        static public string secretKey= "dcfyvgubhinnoiuhgytfytguo[pij[oijomkp";
        public string GenerateToken(User user)
        {
            Claim[] claims = { new("UserId", user.Id.ToString()) };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(5)
                );

            var tokenValue= new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
