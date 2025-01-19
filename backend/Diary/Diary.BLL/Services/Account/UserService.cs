using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Diary.BLL.Services.Account
{
    public class UserService : BaseService, IUserService
    {
        private readonly IJwtService _jwtService;
        private readonly ISessionService _sessionService;
        private readonly UserIdStorage _userIdStorage;
        private readonly double _softDeletePeriod;

        public UserService(DiaryContext context, IJwtService jwtService,
            ISessionService sessionService, UserIdStorage userIdStorage, IConfiguration configuration) : base(context)
        {
            _jwtService = jwtService;
            _sessionService = sessionService;
            _userIdStorage = userIdStorage;
            if (double.TryParse(configuration["Account:SoftDeleteTimeHours"], out double result))
                _softDeletePeriod = 48;
            _softDeletePeriod = result;
        }
        public async Task CreateUser(UserCreateDTO userCreateDTO)
        {
            var user = new User
            {
                Nickname = userCreateDTO.Name,
                Email = userCreateDTO.Email,
                Role = UserRoles.User
            };

            var passHash = new PasswordHasher<User>().HashPassword(user, userCreateDTO.Password);
            user.PasswordHash = passHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> AuthenticateAsync(UserLoginDTO userLoginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);
            if (user != null)
            {
                var hashVerifyResult = user != null
                    ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password) : PasswordVerificationResult.Failed;

                if (hashVerifyResult == PasswordVerificationResult.Success)
                {
                    var session = await _sessionService.CreateSessionAsync(user);
                    var token = _jwtService.GenerateJwtToken(user, session);
                    return _jwtService.GenerateJwtToken(user, session);
                }
            }
            return null;
        }

        public async Task DeleteAccountAsync(AccountDeletionDTO account)
        {
            var userId = Guid.Parse(_userIdStorage.CurrentUserId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var hashVerifyResult = user != null
                    ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, account.Password) : PasswordVerificationResult.Failed;

                if (hashVerifyResult == PasswordVerificationResult.Success)
                {
                    MarkAccountForDelitionAsync(userId);
                }
            }
        }

        public async Task RestoreAccountAsync(AccountDeletionDTO account)
        {
            var userId = Guid.Parse(_userIdStorage.CurrentUserId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                var hashVerifyResult = user != null
                    ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, account.Password) : PasswordVerificationResult.Failed;

                if (hashVerifyResult == PasswordVerificationResult.Success)
                {
                    if (user.MarkedForDeletionAt > DateTime.Now.AddHours(-_softDeletePeriod))
                    {
                        user.Status = AccountStatus.Active;
                        user.MarkedForDeletionAt = default;
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        public async Task DeleteExpiredAccountsAsync()
        {
            var usersToDelete = await _context.Users.Where(u => u.Status == AccountStatus.MarkedForDeletion
                && u.MarkedForDeletionAt < DateTime.Now).ToListAsync();
            _context.Users.RemoveRange(usersToDelete);
            await _context.SaveChangesAsync();
        }

        private async void MarkAccountForDelitionAsync(Guid userId)
        {
            var acc = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            acc.Status = AccountStatus.MarkedForDeletion;
            acc.MarkedForDeletionAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }


    }
}
