using Diary.BLL.DTO.Account;
using Diary.DAL.Entities;

namespace Diary.BLL.Services.Interfaces
{
    public interface IInvitationService
    {
        Task<Invitation> CreateInviteAsync(InvitationDTO invitationDTO);
        Task MarkTokenAsUsedAsync(string token);
        Task SendInvitationAsync(string email, string token);
        Task<bool> ValidateInviteTokenAsync(string token);
    }
}