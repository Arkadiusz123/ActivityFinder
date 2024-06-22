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

        public ActivityController(ILogger<ActivityController> logger, AppDbContext context)
        {
            _logger = logger;
            _addressSearch = new AddressSearch(context);
            _activityService = new ActivityService(context);
            _userService = new UserService(context);
        }

        [HttpGet]
        [AllowAnonymous]//tylko na test TODO
        public IActionResult GetList([FromQuery]ActivityPaginationSettings settings)
        {
            var userName = User.Identity.Name;

            var result = _activityService.GetPagedVm(settings, userName, ActivityMapper.SelectVmExpression(userName));
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var activityResult = _activityService.GetById(id);

            if (!activityResult.Success)
                return BadRequest(activityResult.Message);

            return Ok(ActivityMapper.ToDTO(activityResult.Value));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ActivityDTO activity)
        {
            var addressResult = await _addressSearch.GetAddressByOsmId(activity.Address.OsmId!);

            if (!addressResult.Success)
                return NotFound(addressResult.Message);

            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            _activityService.Add(ActivityMapper.ToActivity(activity, addressResult.Value, userResult.Value));
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] ActivityDTO activity)
        {
            if (!activity.Id.HasValue)
                return BadRequest("Nie podano id");

            var addressResult = await _addressSearch.GetAddressByOsmId(activity.Address.OsmId!);

            if (!addressResult.Success)
                return NotFound(addressResult.Message);

            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            _activityService.Edit(ActivityMapper.ToActivity(activity, addressResult.Value, userResult.Value));
            return Ok();
        }

        [HttpPost]
        [Route("join/{id}")]
        public IActionResult JoinToActivity(int id)
        {
            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var joinResult = _activityService.JoinUser(userResult.Value, id);

            if (!joinResult.Success)
                return BadRequest(joinResult.Message);

            return Ok();
        }

        [HttpPost]
        [Route("leave/{id}")]
        public IActionResult LeaveActivity(int id)
        {
            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var removeResult = _activityService.RemoveFromActivity(userResult.Value, id);

            if (!removeResult.Success)
                return BadRequest(removeResult.Message);

            return Ok();
        }
    }
}
