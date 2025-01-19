using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Diary.DAL.Tests
{
    public class InvitationServiceTests : IDisposable
    {
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly InvitationService _invitationService;
        private readonly DbContextOptions<DiaryContext> _options;
        private readonly DiaryContext _context;

        public InvitationServiceTests()
        {
            _options = new DbContextOptionsBuilder<DiaryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DiaryContext(_options);
            _emailServiceMock = new Mock<IEmailService>();
            _invitationService = new InvitationService(_context, _emailServiceMock.Object);
        }

        [Fact]
        public async Task CreateInviteAsync_Should_Add_Entity_To_Db()
        {
            //Arrange
            var invitationDTO = new InvitationDTO { Email = "email@dot.com" };

            //Act
            await _invitationService.CreateInviteAsync(invitationDTO);

            //Assert
            var result = await _context.Invitations.FirstOrDefaultAsync(x => x.Email == invitationDTO.Email);
            Assert.NotNull(result);
            Assert.Equal(invitationDTO.Email, result.Email);
        }
        [Fact]
        public async Task ValidateInviteTokenAsync_Should_Return_True_When_Token_Valid()
        {
            //Arrange
            var invitation = new Invitation
            {
                Email = "email@dot.com",
                Token = Guid.NewGuid().ToString(),
                IsUsed = false,
                ExpirationDate = DateTime.Now.AddHours(2)
            };
            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();

            //Act
            var result = _invitationService.ValidateInviteTokenAsync(invitation.Token).Result;

            //Assert
            Assert.True(result);

        }
        [Fact]
        public async Task MarkTokenAsUsedAsync_Should_Set_IsUsed_To_True()
        {
            //Arrange
            var invitation = new Invitation
            {
                Email = "email@dot.com",
                Token = Guid.NewGuid().ToString(),
                IsUsed = false,
                ExpirationDate = DateTime.Now.AddHours(2)
            };
            await _context.Invitations.AddAsync(invitation);
            await _context.SaveChangesAsync();

            //Act
            await _invitationService.MarkTokenAsUsedAsync(invitation.Token);

            //Assert
            var result = _context.Invitations.FirstOrDefault(i => i.Token == invitation.Token);
            Assert.NotNull(result);
            Assert.True(result.IsUsed);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
