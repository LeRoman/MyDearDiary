using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.BLL.Extensions;
using Diary.BLL.Mappers;
using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Account;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Services
{
    public class RecordsService : BaseService, IRecordsService
    {
        private readonly UserIdStorage _userIdStorage;
        private readonly IImageService _imageService;
        private readonly IAesEncryptionService _encryptionService;

        public RecordsService(DiaryContext diaryContext, UserIdStorage userIdStorage,
            IImageService imageService, IAesEncryptionService encryptionService) : base(diaryContext)
        {
            _userIdStorage = userIdStorage;
            _imageService = imageService;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<Record>> GetRecords()
        {
            return await _context.Records.ToListAsync();
        }

        public async Task AddRecordAsync(CreateRecordDTO recordDTO)
        {
            var record = new Record()
            {
                UserId = Guid.Parse(_userIdStorage.CurrentUserId),
                Content = _encryptionService.Encrypt(recordDTO.Content)
            };

            if (recordDTO.Images != null)
            {
                record.Images = new List<Image>();

                foreach (var image in recordDTO.Images)
                {
                    var savedImage = await _imageService.SaveImageAsync(image);
                    savedImage.RecordId = record.Id;
                    record.Images.Add(savedImage);
                }
            }

            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public PagedResult<RecordDTO> GetUserRecords(RecordsListParams recordFilter, PageParams pageParams)
        {
            var userId = Guid.Parse(_userIdStorage.CurrentUserId);
            var record =
                _context
                .Records
                .Include(record=>record.Images)
                .Where(x => x.UserId == userId)
                .FilterByDate(recordFilter)
                .Decrypt(_encryptionService)
                .FilterByContent(recordFilter)
                .Select(x => RecordMapper.ToDTO(x))
                .ToPaged(pageParams);
                

            return record;
        }

        public async Task DeleteRecordAsync(Record record)
        {
            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<Record> GetRecordByIdAsync(string recordId)
        {
            var guidId = Guid.Parse(recordId);
            return await _context.Records.FirstOrDefaultAsync(x => x.Id == guidId);
        }

        public async Task<bool> CanDeleteRecordAsync(string recordId)
        {
            var guidId = Guid.Parse(recordId);
            return await _context.Records
                .AnyAsync(x => x.Id == guidId && x.CreatedAt >= DateTime.Now.AddDays(-2));
        }
    }
}


