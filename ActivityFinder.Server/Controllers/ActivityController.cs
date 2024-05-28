using ActivityFinder.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    public class ActivityController : ControllerBase
    {
        [HttpPost, Route("")]
        public IActionResult Create([FromBody]ActivityDTO activity)
        {
            return null;
        }
    }
}
