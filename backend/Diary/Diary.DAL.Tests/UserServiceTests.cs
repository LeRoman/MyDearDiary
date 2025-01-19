using Diary.BLL.DTO.Account;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Diary.DAL.Tests
{
    public class UserServiceTests : IDisposable
    {
        private readonly DbContextOptions<DiaryContext> _options;
        private readonly DiaryContext _context;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<ISessionService> _sessionServiceMock;
        private readonly Mock<UserIdStorage> _userIdStorageMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _options = new DbContextOptionsBuilder<DiaryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DiaryContext(_options);
            _jwtServiceMock = new Mock<IJwtService>();
            _jwtServiceMock
                .Setup(x => x.GenerateJwtToken(It.IsAny<User>(), It.IsAny<Session>()))
                .Returns(new string("token"));
            _sessionServiceMock = new Mock<ISessionService>();
            _sessionServiceMock
                .Setup(x => x.CreateSessionAsync(It.IsAny<User>()))
                .ReturnsAsync(new Session());

            _userIdStorageMock = new Mock<UserIdStorage>();
            _userIdStorageMock.Object.SetUserId(Guid.NewGuid().ToString());
            _configurationMock = new Mock<IConfiguration>();
            _userService = new UserService(_context,
                _jwtServiceMock.Object,
                _sessionServiceMock.Object,
                _userIdStorageMock.Object,
                _configurationMock.Object
                );
        }


        [Fact]
        public async Task CreateUser_Should_Add_User_To_Db()
        {
            //Arrange
            var userDto = new UserCreateDTO()
            {
                Email = "mail@dot.com",
                Name = "userName",
                Password = "pass",
                Token = "asdasdv34"
            };

            //Act
            await _userService.CreateUser(userDto);

            //Assert
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Nickname == userDto.Name);
            Assert.NotNull(result);
            Assert.Equal(userDto.Email, result.Email);
        }

        [Fact]
        public async Task Authenticate_Should_Return_Token_If_User_Validated()
        {
            //Arrange
            var password = "mypassword";
            var user = new User()
            {
                Email = "mail@dot.com"
            };
            var passHash = new PasswordHasher<User>().HashPassword(user, password);
            user.PasswordHash = passHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var userLoginDto = new UserLoginDTO() { Email = "mail@dot.com", Password = password };

            //Act
            var result = await _userService.AuthenticateAsync(userLoginDto);

            //Assert
            Assert.NotNull(result);
            Assert.True(result is string);
        }

        [Fact]
        public async Task DeleteAccountAsync_Should_Mark_Account_For_Delition()
        {
            //Arrange
            var password = "mypassword";
            var user = new User() { Nickname = "Quake" };
            _userIdStorageMock.Object.SetUserId(user.Id.ToString());
            var passHash = new PasswordHasher<User>().HashPassword(user, password);
            user.PasswordHash = passHash;
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var accountDeletionDTO = new AccountDeletionDTO() { Password = password };

            //Act
            await _userService.DeleteAccountAsync(accountDeletionDTO);

            //Assert
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Nickname == user.Nickname);
            Assert.NotNull(result);
            Assert.Equal(Enums.AccountStatus.MarkedForDeletion, result.Status);
        }

        [Fact]
        public async Task RestoreAccountAsync_Should_Set_Status_To_Active()
        {
            //Arrange
            var password = "mypassword";
            var user = new User() { Nickname = "Quake" };
            _userIdStorageMock.Object.SetUserId(user.Id.ToString());
            var passHash = new PasswordHasher<User>().HashPassword(user, password);
            user.PasswordHash = passHash;

            user.Status = Enums.AccountStatus.MarkedForDeletion;
            user.MarkedForDeletionAt = DateTime.Now.AddHours(2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            var accountDeletionDTO = new AccountDeletionDTO() { Password = password };

            //Act
            await _userService.RestoreAccountAsync(accountDeletionDTO);

            //Assert
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Nickname == user.Nickname);
            Assert.NotNull(result);
            Assert.Equal(Enums.AccountStatus.Active, result.Status);
        }

        [Fact]
        public async Task DeleteExpiredAccountsAsync_Should_Remove_User_From_Db()
        {
            //Arrange
            var user = new User() { Nickname = "Quake" };
            user.Status = Enums.AccountStatus.MarkedForDeletion;
            user.MarkedForDeletionAt = DateTime.Now.AddHours(-2);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            //Act
            await _userService.DeleteExpiredAccountsAsync();

            //Assert
            var result = await _context.Users.FirstOrDefaultAsync(x => x.Nickname == user.Nickname);
            Assert.Null(result);

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
