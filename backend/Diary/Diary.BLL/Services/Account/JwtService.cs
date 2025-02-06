using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Diary.BLL.Services.Account
{
    public class JwtService : BaseService, IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(DiaryContext context, IConfiguration configuration) : base(context)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(User user, Session session)
        {
            double jwtLifeTimeHours = 0.25;
            if (double.TryParse(_configuration["Jwt:LifeTimeHours"], out double result))
                jwtLifeTimeHours = result;
            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Role.ToString()),
                new Claim("Status",user.Status.ToString()),
                new Claim("SessionId",session.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(jwtLifeTimeHours),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
