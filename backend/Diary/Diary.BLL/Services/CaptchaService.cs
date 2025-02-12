using Diary.BLL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Diary.BLL.Services
{
    public class CaptchaService : ICapthaService
    {
        private readonly IConfiguration _configuration;

        public CaptchaService(IConfiguration configuration)
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
            int width = 150;
            int height = 50;

            using (var image = new Image<Rgba32>(width, height))
            {
                var font = SystemFonts.CreateFont("DejaVu Sans", 24, FontStyle.Bold);

                var textSize = TextMeasurer.MeasureSize(captchaText, new TextOptions(font));
                var textPosition = new PointF((width - textSize.Width) / 2, (height - textSize.Height) / 2);

                image.Mutate(ctx =>
                {
                    ctx.Fill(Color.White);
                    ctx.DrawText(captchaText, font, Color.Black, textPosition);
                    ctx.DrawLine(Color.Gray, 2, new PointF(0, 0), new PointF(width, height));
                    ctx.DrawLine(Color.Gray, 2, new PointF(0, height), new PointF(width, 0));
                });

                using (var stream = new MemoryStream())
                {
                    image.SaveAsPng(stream);
                    return Convert.ToBase64String(stream.ToArray());
                }
            }
        }

    }
}

