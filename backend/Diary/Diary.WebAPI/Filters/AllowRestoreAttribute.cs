namespace Diary.WebAPI.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AllowRestoreAttribute:Attribute
    {
    }
}
