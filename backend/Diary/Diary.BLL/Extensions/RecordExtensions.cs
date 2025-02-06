using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Entities;

namespace Diary.BLL.Extensions
{
    public static class RecordExtensions
    {
        public static IQueryable<Record> FilterByDate(this IQueryable<Record> query, RecordsListParams recordFilter)
        {
            var filter = recordFilter;

            if (filter.StartDate.HasValue & filter.EndDate.HasValue)
            {
                var startDate = filter.StartDate.Value.Date;
                var endDate = filter.EndDate.Value.Date.AddDays(1);
                query = query.Where(y => y.CreatedAt >= startDate &&
                     y.CreatedAt <= endDate);
            }

            return query.OrderByDescending(k=>k.CreatedAt);
        }

        public static IEnumerable<Record> FilterByContent(this IEnumerable<Record> query,RecordsListParams recordFilter)
        {
            var filter = recordFilter;
            if (!string.IsNullOrEmpty(filter.SearchFragment))
                query = query.Where(x => x.Content.Contains(filter.SearchFragment));

            return query;
        }
        public static PagedResult<RecordDTO> ToPaged(this IEnumerable<RecordDTO> query, PageParams pageParams)
        {
            var count=query.Count();

            var page = pageParams.Page ?? 1;
            var pageSize = pageParams.PageSize ?? 5;

            var skip = (page - 1) * pageSize;
            var data = query
                .Skip(skip)
                .Take(pageSize).ToArray();
            return new PagedResult<RecordDTO> (data,count);
        }

        public static IEnumerable<Record> Decrypt(this IEnumerable<Record> query, IAesEncryptionService decryptor)
        {
            foreach (var record in query)
            {
                record.Content = decryptor.Decrypt(record.Content);
                //yield return record;
            }
            return query;
        }
    }
}
