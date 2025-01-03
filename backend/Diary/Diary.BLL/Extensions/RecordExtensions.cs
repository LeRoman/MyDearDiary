using Diary.BLL.DTO;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Diary.BLL.Extensions
{
    public static class RecordExtensions
    {
        public static IQueryable<Record> Filter(this IQueryable<Record> query, RecordFilter recordFilter)
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

        public static async Task<IEnumerable<Record>> ToPagedAsync(this IQueryable<Record> query, PageParams pageParams)
        {
            var page = pageParams.Page ?? 1;
            var pageSize = pageParams.PageSize ?? 5;

            var skip = (page - 1) * pageSize;

            return await query
                .Skip(skip)
                .Take(pageSize)
                .ToArrayAsync();

        }
    }
}
