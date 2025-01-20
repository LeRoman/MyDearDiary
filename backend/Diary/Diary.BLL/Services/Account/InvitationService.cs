using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Diary.BLL.Services.Account
{
    public class InvitationService : BaseService, IInvitationService
    {
        private readonly IEmailService _emailService;

        public InvitationService(DiaryContext diaryContext, IEmailService emailService) : base(diaryContext)
        {
            _emailService = emailService;
        }

        public async Task<Invitation> CreateInviteAsync(InvitationDTO invitationDTO)
        {
            var invitation = new Invitation
            {
                Email = invitationDTO.Email,
                Token = GenerateToken(),
                IsUsed = false,
                ExpirationDate = DateTime.Now.AddHours(invitationDTO.ExpirationHours)
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            return invitation;
        }

        public async Task SendInvitationAsync(string email, string token)
        {
            var subject = "Invitation to the system";
            var body = $"Use for registation: https://localhost:7094/registration?token={token}";
            await _emailService.SendAsync(email, subject, body);
        }

        public async Task<bool> ValidateInviteTokenAsync(string token)
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

        string GenerateToken()
        {
            var tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes);
        }
    }
}
