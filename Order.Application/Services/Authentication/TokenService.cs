using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Order.Application.Interfaces.Authentication;
using Order.Core.Entities;
using Order.Core.Exceptions;

namespace Order.Application.Services.Authentication;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("SignInTime", DateTime.Now.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name),
        };

        var secretKey = _configuration["jwtSecretKey"] ?? throw new JwtKeyNotFoundException("No secret key found.");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "https://ltdluka.ge/",
            audience: "https://ltdluka.ge/",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}