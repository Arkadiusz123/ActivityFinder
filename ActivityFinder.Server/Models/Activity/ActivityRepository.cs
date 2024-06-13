using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    interface IActivityRepository : IGenericRepository<Activity>
    {
        IQueryable<Activity> GetFilteredQuery(string? filter, string state);
        IQueryable<Activity> OrderQuery(IQueryable<Activity> query, string column, bool asc);
    }

    public class ActivityRepository : GenericRepository<Activity>, IActivityRepository
    {
        public ActivityRepository(AppDbContext context) : base(context)
        {             
        }

        public IQueryable<Activity> GetFilteredQuery(string? filter, string state)
        {
            var query = _context.Set<Activity>().Where(x => x.Address.State == state);

            if (!string.IsNullOrEmpty(filter))
            {
                var filterArray = filter.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                query = query
                    .Where(x => filterArray.All(y => (x.Address.Name + x.Address.Town + x.Address.Road + x.Address.HouseNumber + x.Address.County + x.Address.State
                    + x.Title + x.Date.ToString()).ToLower().Contains(y)));
            }

            return query;
        }

        public IQueryable<Activity> OrderQuery(IQueryable<Activity> query, string column, bool asc)
        {
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
}
