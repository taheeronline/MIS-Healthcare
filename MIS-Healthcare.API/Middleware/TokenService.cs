using Microsoft.IdentityModel.Tokens;
using MIS_Healthcare.API.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MIS_Healthcare.API.Middleware
{
    public interface iTokenService
    {
        string GenerateToken(User user);
    }

    public class TokenService : iTokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.UserType)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
