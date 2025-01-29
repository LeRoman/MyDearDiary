namespace Diary.BLL.Services.Interfaces
{
    public interface ICapthaService
    {
        string GenerateCaptchaImageAsBase64(string captchaText);
        string GenerateRandomText(int length);
        string GenerateToken(string captchaText);
        string? ValidateToken(string token);
    }
}