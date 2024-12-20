using Diary.DAL.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.DAL.Entities
{
    internal class User : BaseEntity
    {
        public string Nickname { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
    }
}
