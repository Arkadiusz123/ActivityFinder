using ActivityFinder.Server.Database;
using System.Linq.Expressions;

namespace ActivityFinder.Server.Models
{
    public interface ICommentService
    {
        ValueResult<Comment> AddComment(Comment comment);
        ValueResult<Comment> EditComment(Comment comment);
        ValueResult<IEnumerable<T>> GetDisplayList<T>(string userId, int activityId, Expression<Func<Comment, T>> selectExpression);
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IActivityRepository _activityRepository;

        public CommentService(AppDbContext context)
        {
            _commentRepository = new CommentRepository(context);
            _activityRepository = new ActivityRepository(context);
        }

        public ValueResult<Comment> AddComment(Comment comment) 
        {
            var validateResult = Validate(comment);

            if (!validateResult.Success)
                return new ValueResult<Comment>(false, validateResult.Message);

            _commentRepository.Add(comment);
            _commentRepository.SaveChanges();

            return new ValueResult<Comment>(comment, true);
        }

        public ValueResult<Comment> EditComment(Comment comment)
        {
            comment.Activity = _commentRepository.GetAttachedActivity(comment.CommentId);
            var validateResult = Validate(comment);

            if (!validateResult.Success)
                return new ValueResult<Comment>(false, validateResult.Message);

            _commentRepository.Edit(comment, comment.CommentId);
            _commentRepository.SaveChanges();

            return new ValueResult<Comment>(comment, true);
        }

        public ValueResult<IEnumerable<T>> GetDisplayList<T>(string userId, int activityId, Expression<Func<Comment, T>> selectExpression)
        {
            if (!_activityRepository.CreatedOrJoined(userId, activityId))
                return new ValueResult<IEnumerable<T>>(false, "Użytkownik nie dołączył do wydarzenia");

            var data = _commentRepository.GetDisplayList(activityId, selectExpression);
            return new ValueResult<IEnumerable<T>>(data, true);
        }

        private Result Validate(Comment comment)
        {
            var isEdit = comment.CommentId != 0;

            if (!_activityRepository.CreatedOrJoined(comment.User.Id, comment.Activity.ActivityId))
                return new Result(false, "Użytkownik nie dołączył do wydarzenia");

            if (isEdit) 
            {
                if (!_commentRepository.CanEdit(comment.User.UserName, comment.CommentId))
                    return new Result(false, "Nie znaleziono komentarza");
            }

            return new Result(true);
        }       
    }
}
