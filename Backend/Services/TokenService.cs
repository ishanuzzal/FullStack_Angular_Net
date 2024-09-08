using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyProject.Context;
using MyProject.Interfaces;
using MyProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyProject.Services
{
    public class Auth : IAuthentication
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly AppDbContext _context;
        public Auth(IConfiguration config, AppDbContext appDbContext)
        {
            _config = config;
            _context = appDbContext;
            _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["JWT:Signingkey"]));
        }
        public string CreateToken(AppUser appuser, string Email)
        {
            var claim = new List<Claim>
            {
                 new Claim(ClaimTypes.Email, Email),
                 new Claim(JwtRegisteredClaimNames.GivenName,appuser.UserName),

            };

            var creeds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creeds,
                Issuer = _config["jwt:Issuer"],
                Audience = _config["jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptior);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GetUserId(string UserName)
        {
            var result =  await _context.AppUsers.FirstOrDefaultAsync(e => e.UserName==UserName);
            return result.Id;
        }
    }
}
