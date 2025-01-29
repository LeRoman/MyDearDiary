using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.DAL.Entities;

namespace Diary.BLL.Services.Interfaces
{
    public interface IRecordsService
    {
        Task AddRecordAsync(CreateRecordDTO recordDTO);
        Task<bool> CanDeleteRecordAsync(string recordId);
        Task DeleteRecordAsync(Record record);
        Task<Record> GetRecordByIdAsync(string recordId);
        Task<IEnumerable<Record>> GetRecords();
        PagedResult<RecordDTO> GetUserRecords(RecordsListParams recordFilter, PageParams pageParams);
    }
}