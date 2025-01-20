using Diary.BLL.DTO;
using Diary.BLL.DTO.Record;
using Diary.BLL.Exceptions;
using Diary.BLL.Services.Interfaces;
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
        private readonly IRecordsService _recordService;

        public RecordController(IRecordsService recordsService)
        {
            _recordService = recordsService;
        }

        /// <summary>
        /// Get all records of current user
        /// </summary>
        /// <returns> parametrized list of records</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Record>>> GetRecords(
            [FromQuery] RecordsListParams recordFilter,
            [FromQuery] PageParams pageParams
            )
        {
            var records = _recordService.GetUserRecords(recordFilter, pageParams);
            return Ok(records);
        }

        /// <summary>
        /// Add record as current user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> AddRecord([FromForm] CreateRecordDTO recordDTO)
        {
            await _recordService.AddRecordAsync(recordDTO);
            return Ok();
        }

        /// <summary>
        /// Delete record by ID
        /// </summary>
        [HttpDelete]
        public async Task<ActionResult> DeleteRecord(string recordId)
        {
            var record = await _recordService.GetRecordByIdAsync(recordId);
            if (record is null) throw new NotFoundException("Record not found");

            var canBeDeleted = await _recordService.CanDeleteRecordAsync(recordId);
            if (!canBeDeleted) throw new BadRequestException("Cannot delete record older than 2 days");

            await _recordService.DeleteRecordAsync(record);
            return NoContent();

        }
    }
}
