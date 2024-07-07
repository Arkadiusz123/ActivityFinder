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
        [AllowAnonymous]
        public IActionResult GetList([FromQuery]ActivityPaginationSettings settings)
        {
            var userName = User.Identity.Name;

            var result = _activityService.GetPagedVm(settings, userName, ActivityMapper.SelectVmExpression(userName));
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }

        [HttpGet]
        [Route("user")]
        public IActionResult GetUsersActivities()
        {
            var userName = User.Identity.Name;
            var userResult = _userService.GetByName(userName);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var result = _activityService.GetUsersActivities(userResult.Value, ActivityMapper.SelectVmExpression(userName));
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
                return NotFound(activityResult.Message);

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

            var createResult = _activityService.Add(ActivityMapper.ToActivity(activity, addressResult.Value, userResult.Value));

            if (!createResult.Success)
                return BadRequest(createResult.Message);

            return Ok(ActivityMapper.ToDTO(createResult.Value));
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

            var editResult = _activityService.Edit(ActivityMapper.ToActivity(activity, addressResult.Value, userResult.Value));

            if (!editResult.Success)
                return BadRequest(editResult.Message);

            return Ok(ActivityMapper.ToDTO(editResult.Value));
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _activityService.Delete(id, User.Identity.Name);

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
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
