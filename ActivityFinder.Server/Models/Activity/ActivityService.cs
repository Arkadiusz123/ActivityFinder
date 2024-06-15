using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    interface IActivityService<TVm>
    {
        Result<Activity> Add(Activity activity);
        Result<Activity> Edit(Activity activity);
        Result<TVm> GetPagedVm(ActivityPaginationSettings settings, string userName);
        Result<Activity> GetById(int id);
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
            //some validations
            _repository.Add(activity);
            _repository.SaveChanges();

            _result.SetSuccess(activity);
            return _result;
        }

        public Result<Activity> Edit(Activity activity)
        {
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

        private string? PrepareAddressForFilter(string? address)
        {
            if (string.IsNullOrEmpty(address))
                return address;

            return address.Trim().Replace(",", "").Replace(".", "").Replace("ul", "");
        }
    }
}
