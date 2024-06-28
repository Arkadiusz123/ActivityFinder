using ActivityFinder.Server.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ActivityFinder.Server.Models
{
    public interface ICommentClient
    {
        Task ReceiveComment(CommentVm comment);
        Task EditComment(CommentVm comment);
        Task DeleteComment(int id);
    }

    [Authorize]
    public class CommentHub : Hub<ICommentClient>
    {
        private readonly IUserService _userService;
        private readonly IActivityRepository _activityRepository;

        public CommentHub(AppDbContext context)
        {
            _userService = new UserService(context);
            _activityRepository = new ActivityRepository(context);
        }

        public async Task JoinEventGroup(string activityId)
        {
            var user = _userService.GetByName(Context.User.Identity.Name);
            if (!user.Success)
                return;

            if (!_activityRepository.CreatedOrJoined(user.Value.Id, int.Parse(activityId)))
                return;

            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
        }

        public async Task LeaveEventGroup(string eventId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, eventId);
        }
    }
}
