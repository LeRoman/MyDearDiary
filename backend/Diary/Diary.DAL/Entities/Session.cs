using Diary.DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Entities
{
    internal class Session : BaseEntity
    {
        public Guid UserId { get; private set; }
        public DateTime? ExpiryAt { get; private set; }
    }
}
