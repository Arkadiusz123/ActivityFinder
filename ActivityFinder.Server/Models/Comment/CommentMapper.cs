
namespace ActivityFinder.Server.Models
{
    public class CommentMapper
    {
        public static Comment ToComment(string content, ApplicationUser user)
        {
            var comment = new Comment 
            { 
                Content = content,
                User = user
            };
            return comment;
        }

        public static CommentVm ToVm(Comment comment)
        {
            var vm = new CommentVm()
            {
                Id = comment.CommentId,
                Content = comment.Content,
                Date = comment.DbProperties.Created.ToString(ConstValues.DateFormatWithHour),
                UserName = comment.User.UserName
            };
            return vm;
        }
    }
}
