namespace Diary.BLL.Services.Interfaces
{
    public interface IFileStorageService
    {
        void EnsureDirectoryExists(string folderPath);
        Task<string> SaveFileAsync(byte[] file, string folderPath, string fileExtension = ".jpeg");
    }
}