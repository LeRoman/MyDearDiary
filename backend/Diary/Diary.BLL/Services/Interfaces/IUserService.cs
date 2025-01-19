using Diary.BLL.DTO.Account;

namespace Diary.BLL.Services.Interfaces
{
    public interface IUserService
    {
        Task<string?> AuthenticateAsync(UserLoginDTO userLoginDTO);
        Task CreateUser(UserCreateDTO userCreateDTO);
        Task DeleteAccountAsync(AccountDeletionDTO account);
        Task DeleteExpiredAccountsAsync();
        Task RestoreAccountAsync(AccountDeletionDTO account);
    }
}