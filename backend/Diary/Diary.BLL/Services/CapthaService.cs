using Diary.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Diary.BLL.Services
{
    public class CapthaService : ICapthaService
    {
        private readonly IConfiguration _configuration;

        public CapthaService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GenerateToken(string captchaText)
        {
            var claims = new[] { new Claim("Captcha", captchaText) };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal.FindFirst("Captcha")?.Value;
            }
            catch
            {
                return null;
            }
        }

        public string GenerateRandomText(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string GenerateCaptchaImageAsBase64(string captchaText)
        {
            using (var bitmap = new Bitmap(150, 50))
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.White);
                var font = new Font("Arial", 24, FontStyle.Bold);
                var brush = new SolidBrush(Color.Black);
                graphics.DrawString(captchaText, font, brush, 10, 10);

                graphics.DrawLine(new Pen(Color.Gray, 2), new Point(0, 0), new Point(150, 50));
                graphics.DrawLine(new Pen(Color.Gray, 2), new Point(0, 50), new Point(150, 0));
                using (var stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }
    }
}

