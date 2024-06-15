using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    interface IActivityRepository : IGenericRepository<Activity>
    {
        IQueryable<Activity> GetFilteredQuery(string? address, string state, ActivityStatus status, string userName);
        IQueryable<Activity> OrderQuery(IQueryable<Activity> query, string column, bool asc);
    }

    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context)
        {             
        }

        public IQueryable<Activity> GetFilteredQuery(string? address, string state, ActivityStatus status, string userName)
        {
            state = state.ToLower();
            var query = _context.Set<Activity>().Where(x => x.Address.State == state);

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
            else
                throw new ArgumentException("wrong column name");
        }
    }

    public enum ActivityStatus
    {
        All = 1,
        AddedByUser = 2,
        Joined = 3
    }
}
