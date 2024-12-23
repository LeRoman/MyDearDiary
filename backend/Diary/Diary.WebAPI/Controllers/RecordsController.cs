using Microsoft.AspNetCore.Mvc;
using Diary.BLL;
using Diary.BLL.Services;
using Microsoft.AspNetCore.Authorization;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecordsController : ControllerBase
    {
        private readonly RecordsService _recordService;

        public RecordsController(RecordsService recordsService)
        {
            _recordService = recordsService;
        }

        [HttpGet]
        
        public ActionResult GetUsers()
        {
            return Ok();
        }

       
    }
}
