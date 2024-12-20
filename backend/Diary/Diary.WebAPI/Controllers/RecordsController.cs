using Microsoft.AspNetCore.Mvc;

namespace Diary.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RecordsController : ControllerBase
    {
        public RecordsController()
        {

        }
        
        [HttpGet]
        public ActionResult GetUsers()
        {
            return Ok();
        }

        [HttpGet]
        public ActionResult GetUsers(Guid guid)
        {
            return Ok();
        }

    }
}
