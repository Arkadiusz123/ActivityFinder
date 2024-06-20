using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {

    }

    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }


    }
}
