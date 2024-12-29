﻿using Diary.DAL.Entities.Abstract;

namespace Diary.DAL.Entities
{
    public class Record : BaseEntity
    {
        public string Content { get; set; }
        public Guid UserId { get; set; }
    }
}
