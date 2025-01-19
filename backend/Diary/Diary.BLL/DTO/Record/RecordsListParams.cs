namespace Diary.BLL.DTO.Record
{
    public class RecordsListParams
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SearchFragment { get; set; }
    }
}
