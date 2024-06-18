using ActivityFinder.Server.Database;
using Microsoft.EntityFrameworkCore;

namespace ActivityFinder.Server.Models
{
    interface IActivityRepository : IGenericRepository<Activity>
    {
        IQueryable<Activity> GetFilteredQuery(string? address, string state, ActivityStatus status, string userName);
        IQueryable<Activity> OrderQuery(IQueryable<Activity> query, string column, bool asc);
        int JoinedUsersCount(int id);
        bool UserAlreadyJoined(string userId, int activityId);
        void RemoveFromActivity(int activityId, string userId);
    }

    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context)
        {             
        }

        public IQueryable<Activity> GetFilteredQuery(string? address, string state, ActivityStatus status, string userName)
        {
            state = state.ToLower();
            var query = _context.Set<Activity>().Where(x => x.Address.State == state).AsNoTracking();

            if (!string.IsNullOrEmpty(address))
            {
                var addressWordsArray = address.ToLower().Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                query = query
                    .Where(x => addressWordsArray.All(y => (x.Address.Name + x.Address.Town + x.Address.Road + x.Address.HouseNumber ).ToLower().Contains(y)));
            }

            if (status == ActivityStatus.AddedByUser)
            {
                query = query.Where(x => x.Creator.UserName == userName);
            }
            else if (status == ActivityStatus.Joined)
            {
                query = query.Where(x => x.JoinedUsers.Any(y => y.UserName == userName));
            }

            return query;
        }

        public IQueryable<Activity> OrderQuery(IQueryable<Activity> query, string column, bool asc)
        {
            column = column.ToLower();
            if (column == "address")
            {
                if (asc)
                    return query.OrderBy(x => x.Address.Town).ThenBy(x => x.Address.Road).ThenBy(x => x.Address.HouseNumber).ThenBy(x => x.Address.Name);
                else
                    return query.OrderByDescending(x => x.Address.Town).ThenByDescending(x => x.Address.Road)
                        .ThenByDescending(x => x.Address.HouseNumber).ThenByDescending(x => x.Address.Name);
            }
            else if (column == "title")
            {
                return asc ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title);
            }
            else if (column == "date")
            {
                return asc ? query.OrderBy(x => x.Date) : query.OrderByDescending(x => x.Date);
            }
            else if (column == "userscount")
            {
                return asc ? query.OrderBy(x => x.JoinedUsers.Count) : query.OrderByDescending(x => x.JoinedUsers.Count);
            }
            else
                throw new ArgumentException("nieprawidłowa nazwa kolumny");
        }

        public override Activity? FindByKey(object key)
        {
            var id = Convert.ToInt32(key);

            var activity = _context.Activity
                .Include(x => x.Address)
                .Include(x => x.Creator)
                .SingleOrDefault(x => x.ActivityId == id);

            return activity;
        }

        public override void Edit(Activity entity, object key)
        {
            var dbEntity = FindByKey(key);

            if (dbEntity == null)
                throw new ArgumentException("Nie znaleziono obiektu o podanym id");

            if (dbEntity.Creator != entity.Creator)
                throw new ArgumentException("Nie można edytować obiektów innych użytkowników");

            dbEntity.Title = entity.Title;
            dbEntity.Address = entity.Address;
            dbEntity.Description = entity.Description;
            dbEntity.Date = entity.Date;
            dbEntity.OtherInfo = entity.OtherInfo;

            dbEntity.DbProperties.Edited = DateTime.Now;
        }

        public int JoinedUsersCount(int id)
        {
            var result = _context.Activity.AsNoTracking()
                .Where(x => x.ActivityId == id)
                .Select(x => x.JoinedUsers.Count)
                .Single();

            return result;
        }

        public bool UserAlreadyJoined(string userId, int activityId)
        {
            var result = _context.Activity.AsNoTracking()
                .Where(x => x.ActivityId == activityId)
                .SelectMany(x => x.JoinedUsers)
                .Any(x => x.Id == userId);

            return result;
        }

        public void RemoveFromActivity(int activityId, string userId)
        {
            var user = _context.Set<ApplicationUser>().Include(x => x.JoinedActivities).Single(x => x.Id == userId);
            var activity = _context.Activity.Single(x => x.ActivityId == activityId);

            user.JoinedActivities.Remove(activity);
        }
    }

    public enum ActivityStatus
    {
        All = 1,
        AddedByUser = 2,
        Joined = 3
    }
}
