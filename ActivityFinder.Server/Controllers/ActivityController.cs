using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ActivityController : ControllerBase
    {
        private readonly ILogger<ActivityController> _logger;
        private readonly IAddressSearch _addressSearch;
        private readonly IActivityService _activityService;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public ActivityController(ILogger<ActivityController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
            _addressSearch = new AddressSearch(_context);
            _activityService = new ActivityService(_context);
            _userService = new UserService(_context);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ActivityDTO activity)
        {
            var addressResult = _addressSearch.GetAddressByOsmId(activity.Address.OsmId!);

            if (!addressResult.Success)
                return NotFound(new { addressResult.Message });

            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(new { userResult.Message });

            _activityService.Add(activity.ToActivity(addressResult.Value, userResult.Value));
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]//tylko na test
        public IActionResult GetList(int page, int size, string sortField, string sortDirection, 
            string? filter, string state)
        {
            var query = _activityService.GetList();

            if (!string.IsNullOrEmpty(state))
                query = query.Where(x => x.Address.State == state);

            if (!string.IsNullOrEmpty(filter))
            {
                filter = filter.ToLower().Trim().Replace(",", "");
                query = query.Where(x => 
                //x.Address.ToString().Contains("Lusina") ||
                (x.Address.Name + x.Address.Town + x.Address.Road + x.Address.HouseNumber + x.Address.County + x.Address.State).ToLower().Contains(filter)
                || x.Title.Contains(filter)
                || x.Date.ToString().Contains(filter)
                );
            }

            var test = query.ToList();
            return null;
        }
    }
}
