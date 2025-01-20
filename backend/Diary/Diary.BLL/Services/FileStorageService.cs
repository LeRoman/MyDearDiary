using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;

namespace Diary.BLL.Services
{
    public class FileStorageService : BaseService, IFileStorageService
    {
        public FileStorageService(DiaryContext context) : base(context) { }

        public async Task<string> SaveFileAsync(byte[] file, string folderPath, string fileExtension = ".jpeg")
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
