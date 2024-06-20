using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IActivityService<TVm>
    {
        ValueResult<Activity> Add(Activity activity);
        ValueResult<Activity> Edit(Activity activity);
        ValueResult<TVm> GetPagedVm(ActivityPaginationSettings settings, string userName);
        ValueResult<Activity> GetById(int id);
        Result JoinUser(ApplicationUser user, int activityId);
        Result RemoveFromActivity(ApplicationUser user, int activityId);
    }

    public class ActivityService<TVm> : IActivityService<TVm>
    {
        private readonly IActivityRepository _repository;
        private readonly IEntityToVmMapper<Activity,TVm> _mapper;

        public ActivityService(AppDbContext context, IEntityToVmMapper<Activity, TVm> mapper)
        {
            _repository = new ActivityRepository(context);
            _mapper = mapper;
        }

        public ValueResult<Activity> Add(Activity activity) 
        {
            var validateRes = Validate(activity);
            if (!validateRes.Success)
                return new ValueResult<Activity>(false, validateRes.Message);

            _repository.Add(activity);
            _repository.SaveChanges();

            return new ValueResult<Activity>(activity, true);
        }

        public ValueResult<Activity> Edit(Activity activity)
        {
            var validateRes = Validate(activity);
            if (!validateRes.Success)
                return new ValueResult<Activity>(false, validateRes.Message);

            _repository.Edit(activity, activity.ActivityId);
            _repository.SaveChanges();

            return new ValueResult<Activity>(activity, true);
        }

        public ValueResult<TVm> GetPagedVm(ActivityPaginationSettings settings, string userName)
        {
            var query = _repository.GetFilteredQuery(PrepareAddressForFilter(settings.Address), settings.State, settings.Status, userName);
            query = _repository.OrderQuery(query, settings.SortField, settings.Asc);

            var totalCount = query.Count();
            query = _repository.GetDataForPage(query, settings.Page, settings.Size);

            var vm = _mapper.MapListToVm(query, totalCount, userName);

            return new ValueResult<TVm>(vm, true);
        }

        public ValueResult<Activity> GetById(int id)
        {
            var activity = _repository.FindByKey(id);

            if (activity == null)
            {
                return new ValueResult<Activity>(false, "Nie znaleziono obiektu o podanym id");
            }
            return new ValueResult<Activity>(activity, true);
        }

        public Result JoinUser(ApplicationUser user, int activityId)
        {
            if (_repository.UserAlreadyJoined(user.Id, activityId))
            {
                return new Result(false, "Użytkownik już dołączył do wydarzenia");
            }

            var activity = _repository.FindByKey(activityId);
            if (activity == null)
            {
                return new Result(false, "Nie znaleziono obiektu o podanym id");
            }

            var usersCount = _repository.JoinedUsersCount(activityId);

            if (activity.UsersLimit != null && usersCount >= activity.UsersLimit)
            {
                return new Result(false, "Limit uczetników wypełniony");
            }

            activity.JoinedUsers.Add(user);
            _repository.SaveChanges();

            return new Result(true);
        }

        public Result RemoveFromActivity(ApplicationUser user, int activityId)
        {
            if (!_repository.UserAlreadyJoined(user.Id, activityId))
            {
                return new Result(false, "Użytkownik nie dołączył do wydarzenia");
            }

            _repository.RemoveFromActivity(activityId, user.Id);
            _repository.SaveChanges();

            return new Result(true);
        }

        private string? PrepareAddressForFilter(string? address)
        {
            if (string.IsNullOrEmpty(address))
                return address;

            return address.Trim().Replace(",", "").Replace(".", "").Replace("ul", "");
        }

        private Result Validate(Activity activity)
        {
            bool edit = activity.ActivityId != 0;

            if (edit)
            {
                var usersCount = _repository.JoinedUsersCount(activity.ActivityId);

                if (activity.UsersLimit < usersCount)
                    return new Result(false, "Nie można ustawić limitu mniejszego, niż liczba uczestników");
            }
            return new Result(true);
        }
    }
}
