using Diary.BLL.Services.Account;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Diary.DAL.Tests
{
    public class SessionServiceTests : IDisposable
    {
        private readonly DbContextOptions<DiaryContext> _options;
        private readonly Mock<UserIdStorage> _userIdStorageMock;
        private readonly DiaryContext _context;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly SessionService _sessionService;

        public SessionServiceTests()
        {
            _options = new DbContextOptionsBuilder<DiaryContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
            _userIdStorageMock = new Mock<UserIdStorage>();
            _userIdStorageMock.Object.SetUserId(Guid.NewGuid().ToString());
            _context = new DiaryContext(_options);
            _configurationMock = new Mock<IConfiguration>();
            _sessionService = new SessionService(_context, _configurationMock.Object, _userIdStorageMock.Object);
        }

        [Fact]
        public async Task CreateSessionAsync_Should_Add_Session_To_Db()
        {
            //Arrange
            var user = new User();

            //Act
            await _sessionService.CreateSessionAsync(user);

            //Assert
            var result = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == user.Id);
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.UserId);
        }

        [Fact]
        public async Task RevokeSessionsAsync_Should_Set_IsRevoked_True()
        {
            //Arrange
            var session = new Session() { UserId = Guid.Parse(_userIdStorageMock.Object.CurrentUserId) };
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();

            //Act
            await _sessionService.RevokeSessionsAsync();

            //Assert
            var result = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId == session.UserId);
            Assert.NotNull(result);
            Assert.True(result.IsRevoked);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
