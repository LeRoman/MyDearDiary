using Diary.DAL.Entities;
using Microsoft.AspNetCore.Http;

namespace Diary.BLL.Services.Interfaces
{
    public interface IImageService
    {
        Task<Image> SaveImageAsync(IFormFile image);
    }
}