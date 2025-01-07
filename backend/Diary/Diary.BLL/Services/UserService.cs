using Diary.BLL.DTO;
using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Diary.DAL.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Services
{
    public class UserService : BaseService
    {
        private readonly JwtService _jwtService;
        private readonly SessionService _sessionService;
        private readonly UserIdStorage _userIdStorage;

        public UserService(DiaryContext context, JwtService jwtService,
            SessionService sessionService, UserIdStorage userIdStorage) : base(context)
        {
            _jwtService = jwtService;
            _sessionService = sessionService;
            _userIdStorage = userIdStorage;
        }
        public async Task CreateUser(UserCreateDTO userCreateDTO)
        {

            var user = new User
            {
                Nickname = userCreateDTO.Name,
                Email = userCreateDTO.Email,
                Role = DAL.Enums.UserRoles.User
            };

            var passHash = new PasswordHasher<User>().HashPassword(user, userCreateDTO.Password);
            user.PasswordHash = passHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string?> Authenticate(UserLoginDTO userLoginDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDTO.Email);
            if (user != null)
            {
                var hashVerifyResult = (user != null)
                    ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password) : PasswordVerificationResult.Failed;

                if (hashVerifyResult == PasswordVerificationResult.Success)
                {
                    var session = _sessionService.CreateSession(user).Result;
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
                var hashVerifyResult = (user != null)
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
                var hashVerifyResult = (user != null)
                    ? new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, account.Password) : PasswordVerificationResult.Failed;

                if (hashVerifyResult == PasswordVerificationResult.Success)
                {
                    if (user.MarkedForDeletionAt > DateTime.Now.AddDays(-2))
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
