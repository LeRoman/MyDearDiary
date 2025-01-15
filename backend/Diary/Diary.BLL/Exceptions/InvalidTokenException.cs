namespace Diary.BLL.Exceptions
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException() : base("Token is invalid") { }
    }
}
