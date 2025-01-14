using Diary.BLL.DTO;
using Diary.BLL.Services;
using Diary.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecordController : ControllerBase
    {
        private readonly RecordsService _recordService;

        public RecordController(RecordsService recordsService)
        {
            _recordService = recordsService;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Record>>> GetRecords(
            [FromQuery] RecordFilter recordFilter,
            [FromQuery] PageParams pageParams
            )
        {
            var records = await _recordService.GetUserRecords(recordFilter, pageParams);
            return Ok(records);
        }

        [HttpPost]
        public async Task<ActionResult> AddRecord([FromForm] RecordDTO recordDTO)
        {
            await _recordService.AddRecordAsync(recordDTO);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteRecord(string recordId)
        {
            var record = await _recordService.GetRecordByIdAsync(recordId);
            if (record == null)
            {
                return NotFound("Record not found");
            }

            var canBeDeleted = await _recordService.CanDeleteRecordAsync(recordId);
            if (!canBeDeleted)
            {
                return BadRequest("Cannot delete record older than 2 days");
            }

            await _recordService.DeleteRecordAsync(record);
            return NoContent();

        }
    }
}
