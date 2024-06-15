﻿using ActivityFinder.Server.Database;
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
        private readonly IActivityService<ActivityVmWrapper> _activityService;
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public ActivityController(ILogger<ActivityController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
            _addressSearch = new AddressSearch(_context);
            _activityService = new ActivityService<ActivityVmWrapper>(_context, new ActivityMapper());
            _userService = new UserService(_context);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ActivityDTO activity)
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

        [HttpGet]
        [AllowAnonymous]//tylko na test TODO
        public IActionResult GetList([FromQuery]ActivityPaginationSettings settings)
        {
            var result = _activityService.GetPagedVm(settings, User.Identity.Name);     //TODO filter by status
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Value);
        }
    }
}
