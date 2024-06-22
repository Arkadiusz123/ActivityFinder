using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IActivityService _activityService;
        private readonly IHubContext<CommentHub, ICommentClient> _hubContext;

        public CommentController(ILogger<CommentController> logger, AppDbContext context, IHubContext<CommentHub, ICommentClient> hubContext)
        {
            _logger = logger;
            _commentService = new CommentService(context);
            _userService = new UserService(context);
            _activityService = new ActivityService(context);
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("activity/{activityId}")]
        public IActionResult Create(int activityId, [FromBody]CommentDTO comment)
        {
            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var activityResult = _activityService.GetById(activityId);

            if (!activityResult.Success)
                return NotFound(activityResult.Message);

            var createResult = _commentService.AddComment(CommentMapper.ToComment(comment, userResult.Value, activity: activityResult.Value));

            if (!createResult.Success)
                return BadRequest(createResult.Message);

            var vm = CommentMapper.ToVm(createResult.Value);
            _hubContext.Clients.Group(activityId.ToString()).ReceiveComment(vm);
            return Ok(vm);
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Edit(int id, [FromBody]CommentDTO comment)
        {
            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var editResult = _commentService.EditComment(CommentMapper.ToComment(comment, userResult.Value));

            if (!editResult.Success)
                return BadRequest(editResult.Message);

            return Ok(CommentMapper.ToVm(editResult.Value));
        }

        [HttpGet]
        [Route("activity/{activityId}")]
        public IActionResult GetList(int activityId)
        {
            var userResult = _userService.GetByName(User.Identity.Name);

            if (!userResult.Success)
                return NotFound(userResult.Message);

            var listResult = _commentService.GetDisplayList(userResult.Value.Id, activityId, CommentMapper.SelectVmExpression());

            if (!listResult.Success)
                return BadRequest(listResult.Message);

            return Ok(listResult.Value);
        }
    }
}
