using Diary.DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Entities
{
    internal class Record : BaseEntity
    {
        public string Content { get; private set; }
        public User User { get; private set; }
    }
}
