using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    interface IActivityService<TVm>
    {
        Result<Activity> Add(Activity activity);
        Result<Activity> Edit(Activity activity);
        Result<TVm> GetPagedVm(ActivityPaginationSettings settings, string userName);
        Result<Activity> GetById(int id);
        Result<Activity> JoinUser(ApplicationUser user, int activityId);
    }

    public class ActivityService<TVm> : IActivityService<TVm>
    {
        private readonly IActivityRepository _repository;
        private readonly Result<Activity> _result;
        private readonly IEntityToVmMapper<Activity,TVm> _mapper;

        public ActivityService(AppDbContext context, IEntityToVmMapper<Activity, TVm> mapper)
        {
            _repository = new ActivityRepository(context);
            _result = new Result<Activity>();
            _mapper = mapper;
        }

        public Result<Activity> Add(Activity activity) 
        {
            Validate(activity);
            if (!_result.Success)
                return _result;

            _repository.Add(activity);
            _repository.SaveChanges();

            _result.SetSuccess(activity);
            return _result;
        }

        public Result<Activity> Edit(Activity activity)
        {
            Validate(activity);
            if (!_result.Success)
                return _result;

            _repository.Edit(activity, activity.ActivityId);
            _repository.SaveChanges();

            _result.SetSuccess(activity);
            return _result;
        }

        public Result<TVm> GetPagedVm(ActivityPaginationSettings settings, string userName)
        {
            var query = _repository.GetFilteredQuery(PrepareAddressForFilter(settings.Address), settings.State, settings.Status, userName);
            query = _repository.OrderQuery(query, settings.SortField, settings.Asc);

            var totalCount = query.Count();
            query = _repository.GetDataForPage(query, settings.Page, settings.Size);

            var vm = _mapper.MapListToVm(query, totalCount, userName);

            var vmResult = new Result<TVm>();
            vmResult.SetSuccess(vm);

            return vmResult;
        }

        public Result<Activity> GetById(int id)
        {
            var activity = _repository.FindByKey(id);

            if (activity == null)
            {
                _result.SetFail("Nie znaleziono obiektu o podanym id");
                return _result;
            }

            _result.SetSuccess(activity);
            return _result;
        }

        public Result<Activity> JoinUser(ApplicationUser user, int activityId)
        {
            if (_repository.UserAlreadyJoined(user.Id, activityId))
            {
                _result.SetFail("Użytkownik już dołączył do wydarzenia");
                return _result;
            }

            var activity = _repository.FindByKey(activityId);
            if (activity == null)
            {
                _result.SetFail("Nie znaleziono obiektu o podanym id");
                return _result;
            }

            var usersCount = _repository.JoinedUsersCount(activityId);

            if (activity.UsersLimit != null && usersCount >= activity.UsersLimit)
            {
                _result.SetFail("Limit uczetników wypełniony");
                return _result;
            }

            activity.JoinedUsers.Add(user);
            _repository.SaveChanges();

            _result.SetSuccess(null);
            return _result;
        }

        private string? PrepareAddressForFilter(string? address)
        {
            if (string.IsNullOrEmpty(address))
                return address;

            return address.Trim().Replace(",", "").Replace(".", "").Replace("ul", "");
        }

        private void Validate(Activity activity)
        {
            bool edit = activity.ActivityId != 0;

            if (edit)
            {
                var usersCount = _repository.JoinedUsersCount(activity.ActivityId);

                if (activity.UsersLimit < usersCount)
                    _result.SetFail("Nie można ustawić limitu mniejszego, niż liczba uczestników");
            }
        }
    }
}
