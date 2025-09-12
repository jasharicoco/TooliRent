using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TooliRent.WebAPI.Auth
{
    public interface IJwtTokenService
    {
        string CreateToken(IdentityUser user, IEnumerable<string>? roles = null);
    }

    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(IdentityUser user, IEnumerable<string>? roles = null)
        {
            var jwt = _configuration.GetSection("Jwt");

            // 🔒 Validera konfiguration
            var keyStr = jwt["Key"] ?? throw new InvalidOperationException("JWT Key is not configured.");
            var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var audience = jwt["Audience"] ?? throw new InvalidOperationException("JWT Audience is not configured.");
            var expireMinutes = double.TryParse(jwt["ExpireMinutes"], out var exp) ? exp : 60;


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(ClaimTypes.Email, user.Email ??= ""),
                new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? "")
            };

            if (roles?.Any() == true)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"] ?? "60")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}


//using Microsoft.AspNetCore.Identity;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace TooliRent.WebAPI.Auth
//{
//    public interface IJwtTokenService
//    {
//        string CreateToken(IdentityUser user, IEnumerable<string>? roles = null, int? customerId = null);
//    }

//    public class JwtTokenService : IJwtTokenService
//    {
//        private readonly IConfiguration _configuration;

//        public JwtTokenService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public string CreateToken(IdentityUser user, IEnumerable<string>? roles = null, int? customerId = null)
//        {
//            var jwt = _configuration.GetSection("Jwt");
//            var keyStr = jwt["Key"]!;
//            var issuer = jwt["Issuer"]!;
//            var audience = jwt["Audience"]!;
//            var expMinutes = double.Parse(jwt["AccessTokenExpireMinutes"] ?? "60");

//            var claims = new List<Claim> {
//        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
//        new Claim(ClaimTypes.Email, user.Email ?? ""),
//        new Claim(ClaimTypes.Name, user.UserName ?? user.Email ?? "")
//    };
//            if (roles != null) foreach (var r in roles) claims.Add(new Claim(ClaimTypes.Role, r));
//            if (customerId.HasValue) claims.Add(new Claim("customerId", customerId.Value.ToString()));

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: issuer, audience: audience, claims: claims,
//                expires: DateTime.UtcNow.AddMinutes(expMinutes), signingCredentials: creds
//            );
//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
