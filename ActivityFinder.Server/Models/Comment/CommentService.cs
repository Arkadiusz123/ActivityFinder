using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface ICommentService
    {
        ValueResult<Comment> AddComment(Comment comment);
    }

    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(AppDbContext context)
        {
            _commentRepository = new CommentRepository(context);
        }

        public ValueResult<Comment> AddComment(Comment comment) 
        {
            _commentRepository.Add(comment);
            _commentRepository.SaveChanges();

            return new ValueResult<Comment>(comment, true);
        }
    }
}
