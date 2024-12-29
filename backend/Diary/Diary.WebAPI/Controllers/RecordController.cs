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

        public async Task<ActionResult<IEnumerable<Record>>> GetRecords()
        {
            var records = await _recordService.GetUserRecords();
            return Ok(records);
        }
        [HttpPost]
        public async Task<ActionResult> AddRecord([FromBody] RecordDTO recordDTO)
        {
            await _recordService.AddRecord(recordDTO);
            return Ok();
        }

    }
}
