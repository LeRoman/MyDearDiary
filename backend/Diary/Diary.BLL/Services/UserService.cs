using Azure.Core;
using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Services
{
    public class UserService : BaseService
    {
        private readonly JwtService _jwtService;

        public UserService(DiaryContext context, JwtService jwtService) : base(context)
        {
            _jwtService = jwtService;
        }
        public async Task CreateUser(string email, string name, string password)
        {

            var user = new User
            {
                Nickname = name,
                Email = email,
                Role = DAL.Enums.UserRoles.User
            };

            var passHash = new PasswordHasher<User>().HashPassword(user, password);
            user.PasswordHash = passHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            var passHash = new PasswordHasher<User>().HashPassword(user, password);
            var hashVerifyResult = (user != null)
                ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password) : PasswordVerificationResult.Failed;

            if (hashVerifyResult == PasswordVerificationResult.Success)
            {
                return _jwtService.GenerateJwtToken(user);
            }

            return null;
        }

    }
}
