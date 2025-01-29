using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.BLL.DTO
{
    public class PagedResult<T>
    {
        public PagedResult(T[]data,int count )
        {
            Data = data;
            Count = count;
        }

        public T[] Data { get; }
        public int Count { get; }
    }
}
