
using System.Linq.Expressions;

namespace ActivityFinder.Server.Models
{
    public class CommentMapper
    {
        public static Comment ToComment(CommentDTO dto, ApplicationUser user, Activity activity = null) //if edit, find activity in db
        {
            var comment = new Comment 
            { 
                CommentId = dto.Id ?? 0,
                Content = dto.Content,
                User = user,
                Activity = activity
            };
            return comment;
        }

        public static CommentVm ToVm(Comment comment)
        {
            var vm = new CommentVm
            {
                Id = comment.CommentId,
                Content = comment.Content,
                Date = comment.DbProperties.Created.ToString(ConstValues.DateFormatWithHour),
                UserName = comment.User.UserName
            };
            return vm;
        }

        public static Expression<Func<Comment, CommentVm>> SelectVmExpression()
        {
            return x => new CommentVm
            {
                Id = x.CommentId,
                Content = x.Content,
                Date = x.DbProperties.Created.ToString(ConstValues.DateFormatWithHour),
                UserName = x.User.UserName
            };
        }
    }
}
