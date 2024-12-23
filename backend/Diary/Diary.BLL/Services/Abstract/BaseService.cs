using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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
