using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.BLL.Services;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Extensions
{
    public static class RecordExtensions
    {
        public static IQueryable<Record> Filter(this IQueryable<Record> query, RecordsListParams recordFilter)
        {
            var filter = recordFilter;
            if (!string.IsNullOrEmpty(filter.SearchFragment))
                query = query.Where(x => x.Content.Contains(filter.SearchFragment));

            if (filter.StartDate.HasValue & filter.EndDate.HasValue)
            {
                var startDate = filter.StartDate.Value.Date;
                var endDate = filter.EndDate.Value.Date.AddDays(1);
                query = query.Where(y => y.CreatedAt >= startDate &&
                     y.CreatedAt <= endDate);
            }

            return query;
        }

        public static IEnumerable<Record> ToPaged(this IQueryable<Record> query, PageParams pageParams)
        {
            var page = pageParams.Page ?? 1;
            var pageSize = pageParams.PageSize ?? 5;

            var skip = (page - 1) * pageSize;

            return  query.Skip(skip)
                .Take(pageSize)
                .ToArray();

        }

        public static IEnumerable<Record> Decrypt(this IEnumerable<Record> query, AesEncryptionService decryptor)
        {
            foreach (var record in query)
            {
                record.Content = decryptor.Decrypt(record.Content);
                yield return record;
            }
        }
    }
}
