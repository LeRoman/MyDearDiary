using Diary.BLL.DTO;
using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Services
{
    public class InvitationService : BaseService
    {
        public InvitationService(DiaryContext diaryContext) : base(diaryContext) { }

        public async Task<Invitation> CreateInviteAsync(InvitationDTO invitationDTO)
        {
            var invitation = new Invitation
            {
                Email = invitationDTO.Email,
                Token = Guid.NewGuid().ToString(),
                IsUsed = false
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            return invitation;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            var invitation = await _context.Invitations
                .FirstOrDefaultAsync(i => i.Token == token && !i.IsUsed);

            return invitation != null;
        }

        public async Task MarkTokenAsUsedAsync(string token)
        {
            var invitation = await _context.Invitations
                .FirstOrDefaultAsync(i => i.Token == token);

            if (invitation != null)
            {
                invitation.IsUsed = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
