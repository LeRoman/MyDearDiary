using Diary.BLL.DTO;
using Diary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                query = query.Where(y => y.CreatedAt >= filter.StartDate &&
            y.CreatedAt <= filter.EndDate);

            return query;
        }
    }
}
