namespace Diary.BLL.DTO
{
    public class PagedResult<T>
    {
        public PagedResult(T[] data, int count)
        {
            Data = data;
            Count = count;
        }

        public T[] Data { get; }
        public int Count { get; }
    }
}
