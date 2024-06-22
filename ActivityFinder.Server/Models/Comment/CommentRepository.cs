﻿using ActivityFinder.Server.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ActivityFinder.Server.Models
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        bool CanEdit(string userName, int id);
        Activity GetAttachedActivity(int commentId);
        IEnumerable<T> GetDisplayList<T>(int activityId, Expression<Func<Comment, T>> selectExpression);
    }

    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context)
        {
        }

        public bool CanEdit(string userName, int id)
        {
            return _context.Comments.Any(x => x.CommentId == id && x.User.UserName == userName);
        }

        public override void Edit(Comment entity, object key)
        {
            var id = (int)key;
            var dbComment = _context.Comments.SingleOrDefault(x => x.CommentId == id);

            dbComment.Content = entity.Content;
            dbComment.DbProperties.Edited = DateTime.Now;
        }

        public Activity GetAttachedActivity(int commentId)
        {
            var comment = _context.Comments.Include(x => x.Activity).SingleOrDefault(x => x.CommentId == commentId);

            return comment.Activity;
        }

        public IEnumerable<T> GetDisplayList<T>(int activityId, Expression<Func<Comment, T>> selectExpression)
        {
            return _context.Comments
                .Where(x => x.ActivityId == activityId)
                .OrderByDescending(x => x.DbProperties.Created)
                .Select(selectExpression)
                .AsEnumerable();
        }

    }
}