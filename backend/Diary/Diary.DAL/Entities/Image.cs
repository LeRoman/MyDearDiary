using Diary.DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Entities
{
    public class Image : BaseEntity
    {
        public string? FileName { get; private set; }
        public Guid RecordId { get; private set; }
    }
}
