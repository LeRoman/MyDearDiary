using Diary.DAL.Context;

namespace Diary.BLL.Services.Abstract
{
    public class BaseService
    {
        private protected readonly DiaryContext _context;

        public BaseService(DiaryContext context)
        {
            _context = context;
        }
    }
}
