using Diary.BLL.Services.Abstract;
using Diary.BLL.Services.Interfaces;
using Diary.DAL.Context;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using SharpImage = SixLabors.ImageSharp;

namespace Diary.BLL.Services
{
    public class ImageService : BaseService, IImageService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly string? _storagePath;

        public ImageService(DiaryContext context, IFileStorageService fileStorageService, IConfiguration configuration) : base(context)
        {
            _fileStorageService = fileStorageService;
            _storagePath = configuration["storage:path"];
        }

        public async Task<Image> SaveImageAsync(IFormFile image)
        {
            var path = _storagePath;
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                var optimizedImage = await OptimizeImageAsync(memoryStream);
                var fileName = await _fileStorageService.SaveFileAsync(optimizedImage, path);
            }
            return new Image
            {
                FileName = Guid.NewGuid().ToString(),
                Path = path,
                Size = image.Length,
                UploadedAt = DateTime.Now,
            };
        }

        private async Task<byte[]> OptimizeImageAsync(Stream imageStream)
        {
            using var image = SharpImage.Image.Load(imageStream);

            var maxWidth = 640;
            var maxHeight = 480;
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new SharpImage.Size(maxWidth, maxHeight)
            }));

            var encoder = new JpegEncoder { Quality = 75 };

            using var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder);

            return outputStream.ToArray();
        }
    }
}
