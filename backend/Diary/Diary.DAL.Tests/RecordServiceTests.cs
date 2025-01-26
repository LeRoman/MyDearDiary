using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.BLL.Services;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Diary.DAL.Tests
{
    public class RecordServiceTests : IDisposable
    {
        private readonly Mock<IImageService> _imageServiceMoq;
        private readonly Mock<IAesEncryptionService> _aesEncryptionServiceMoq;
        private DbContextOptions<DiaryContext> _options;
        private readonly DiaryContext _context;
        private readonly Mock<UserIdStorage> _userIdStorageMoq;
        private readonly RecordsService _recordService;

        public RecordServiceTests()
        {
            _options = new DbContextOptionsBuilder<DiaryContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DiaryContext(_options);

            _userIdStorageMoq = new Mock<UserIdStorage>();
            _userIdStorageMoq.Object.SetUserId(Guid.NewGuid().ToString());

            _imageServiceMoq = new Mock<IImageService>();

            _aesEncryptionServiceMoq = new Mock<IAesEncryptionService>();
            _aesEncryptionServiceMoq.Setup(x => x.Encrypt(It.IsAny<string>())).
                Returns((string encryptedText) => encryptedText);
            _aesEncryptionServiceMoq.Setup(x => x.Decrypt(It.IsAny<string>())).
                Returns((string encryptedText) => encryptedText);

            _recordService = new RecordsService(_context,
                _userIdStorageMoq.Object,
                _imageServiceMoq.Object,
                _aesEncryptionServiceMoq.Object
                );
        }

        [Fact]
        public async Task AddRecordAsync_ShouldAddRecordToDatabase()
        {
            // Arrange
            var record = new CreateRecordDTO { Content = "some content" };

            // Act
            await _recordService.AddRecordAsync(record);

            // Assert
            var result = await _context.Records.FirstOrDefaultAsync(u => u.Content == "some content");
            Assert.NotNull(result);
            Assert.Equal("some content", result.Content);
        }

        [Fact]
        public async Task DeleteRecordAsync_Should_RemoveRecordFromDatabase()
        {
            // Arrange
            var record = new Entities.Record() { Content = "some content" };
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();

            //Act
            await _recordService.DeleteRecordAsync(record);

            //Assert
            var result = await _context.Records.FirstOrDefaultAsync(u => u.Content == "some content");
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserRecords_Should_ReturnsRecords()
        {
            // Arrange
            var id = Guid.Parse(_userIdStorageMoq.Object.CurrentUserId);
            var records = new[] {
            new Entities.Record(){ Content = "some content", UserId=id },
            new Entities.Record(){ Content = "some content", UserId=id },
            new Entities.Record(){ Content = "some content", UserId=id }
            };
            var recordListParamsMock = new Mock<RecordsListParams>();
            var pageParamsMock = new Mock<PageParams>();

            await _context.Records.AddRangeAsync(records);
            await _context.SaveChangesAsync();

            //Act
            var result = _recordService.GetUserRecords(recordListParamsMock.Object, pageParamsMock.Object);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetRecordByIdAsync_Should_ReturnRightRecord()
        {
            //Arange
            var rightRecord = new Entities.Record() { Content = "right content" };
            var wrongRecord = new Entities.Record() { Content = "wrong content" };
            await _context.Records.AddRangeAsync(rightRecord, wrongRecord);
            await _context.SaveChangesAsync();

            //Act
            var result = await _recordService.GetRecordByIdAsync(rightRecord.Id.ToString());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(rightRecord.Id, result.Id);
        }

        [Fact]
        public async Task CanDeleteRecordAsync_Should_ReturnTrue_WhenEntityExist()
        {
            //Arrange
            var record = new Entities.Record() { Content = "some content" };
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();

            //Act
            var result = _recordService.CanDeleteRecordAsync(record.Id.ToString()).Result; ;

            //Assert
            Assert.True(result);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}