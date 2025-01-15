using Diary.BLL.Services.Abstract;
using Diary.DAL.Context;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary.BLL.Services
{
    public class FileStorageService : BaseService
    {
        public FileStorageService(DiaryContext context) : base(context) { }

        internal async Task<string> SaveFileAsync(byte[] file, string folderPath, string fileExtension=".jpeg")
        {
            EnsureDirectoryExists(folderPath);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await stream.WriteAsync(file);
            }

            return uniqueFileName;
        }
        public void EnsureDirectoryExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }
    }
}
