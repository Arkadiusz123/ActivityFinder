using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IActivityService
    {
        public Result<Activity> Add(Activity activity);
    }

    public class ActivityService : IActivityService
    {
        private readonly GenericRepository<Activity> _repository;
        private readonly Result<Activity> _result;

        public ActivityService(AppDbContext context)
        {
            _repository = new GenericRepository<Activity>(context);
            _result = new Result<Activity>();
        }

        public Result<Activity> Add(Activity activity) 
        { 
            //some validations
            _repository.Add(activity);
            _repository.SaveChanges();

            _result.SetSuccess(activity);
            return _result;
        }
    }
}
