namespace Diary.BLL.DTO
{
    public class RecordsListParams
    {
        public DateTime? StartDate {  get; set; }
        public DateTime? EndDate { get; set; }
        public string? SearchFragment { get; set; }
    }
}
