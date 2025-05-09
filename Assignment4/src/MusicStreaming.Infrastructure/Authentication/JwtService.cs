using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicStreaming.Infrastructure.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;

        public JwtService(IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<JwtSecurityToken> GenerateToken(IdentityUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? "unknown"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var userClaim in userClaims)
            {
                authClaims.Add(userClaim);
            }

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

           
            var jwtSecret = _configuration["JWT:Secret"] ?? 
                throw new InvalidOperationException("JWT:Secret is not configured in appsettings.json");
                
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

           
            var issuer = _configuration["JWT:ValidIssuer"] ?? 
                throw new InvalidOperationException("JWT:ValidIssuer is not configured");
                
            var audience = _configuration["JWT:ValidAudience"] ?? 
                throw new InvalidOperationException("JWT:ValidAudience is not configured");
                
            var tokenValidityStr = _configuration["JWT:TokenValidityInMinutes"] ?? "60"; 
            double tokenValidity = Convert.ToDouble(tokenValidityStr);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddMinutes(tokenValidity),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}