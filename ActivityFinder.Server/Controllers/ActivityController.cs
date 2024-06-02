﻿using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;

        public ActivityController(ILogger<ActivityController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody]ActivityDTO activity)
        {
            var test = new AddressSearch().GetAddressByOsmId(activity.Address.OsmId);

            return Ok();
        }
    }
}
