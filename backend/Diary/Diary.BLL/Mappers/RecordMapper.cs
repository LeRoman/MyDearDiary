using Diary.BLL.DTO.Record;
using Diary.DAL.Entities;
using System.Globalization;

namespace Diary.BLL.Mappers
{
    public static class RecordMapper
    {
        public static RecordDTO ToDTO(Record record)
        {
            return new RecordDTO
            {
                Id = record.Id.ToString(),
                Content = record.Content,
                CreatedAt = record.CreatedAt.ToString("dd/MM/yy, HH:mm", CultureInfo.InvariantCulture),
                CanDelete=CanBeDeleted(record.CreatedAt)
            };
        }

        private static bool CanBeDeleted(DateTime createdAt)
        {
            return createdAt >= DateTime.Now.AddDays(-2);
        }
    }
}
