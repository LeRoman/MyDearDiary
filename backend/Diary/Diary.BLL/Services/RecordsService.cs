using Diary.BLL.DTO;
using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Services
{
    public class RecordsService : BaseService
    {
        private readonly UserIdStorage _userIdStorage;

        public RecordsService(DiaryContext diaryContext, UserIdStorage userIdStorage) : base(diaryContext)
        {
            _userIdStorage = userIdStorage;
        }

        public async Task<IEnumerable<Record>> GetRecords()
        {
            return _context.Records.ToList();
        }

        public async Task AddRecord(RecordDTO recordDTO)
        {
            var record = new Record();

            record.UserId = Guid.Parse(_userIdStorage.CurrentUserId);
            record.Content = recordDTO.Content;

            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Record>> GetUserRecords()
        {
            var userId = Guid.Parse(_userIdStorage.CurrentUserId);
            var record = await _context
                .Records
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return record;
        }

    }
}


