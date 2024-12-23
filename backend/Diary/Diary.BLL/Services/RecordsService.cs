using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Diary.DAL.Entities;

namespace Diary.BLL.Services
{
    public class RecordsService : BaseService
    {
        public RecordsService(DiaryContext diaryContext) : base(diaryContext) { }

        public IEnumerable<Record> GetRecords()
        {
            return _context.Records.ToList();
        }
    }
}
