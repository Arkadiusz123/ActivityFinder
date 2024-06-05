using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IAddressSearch _addressSearch;
        private readonly IActivityService _activityService;
        private readonly AppDbContext _context;

        public ActivityController(ILogger<ActivityController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
            _addressSearch = new AddressSearch(_context);
            _activityService = new ActivityService(_context);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ActivityDTO activity)
        {
            var addressResult = _addressSearch.GetAddressByName(activity.Address.OsmId!);

            if (!addressResult.Success)
                return NotFound(addressResult.Message);

            //TODO serive to find users

            //var result = _activityService.Add(activity.ToActivity(addressResult.Value, ));

            return Ok();
        }
    }
}
