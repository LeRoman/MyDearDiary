namespace Diary.BLL.Services.Interfaces
{
    public interface IAesEncryptionService
    {
        string Decrypt(string cipherText);
        string Encrypt(string plainText);
    }
}