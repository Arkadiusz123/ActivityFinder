using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IUserService
    {
        public Result<ApplicationUser> GetByName(string name);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly Result<ApplicationUser> _result;

        public UserService(AppDbContext context)
        {
            _repository = new UserRepository(context);
            _result = new Result<ApplicationUser>();
        }

        public Result<ApplicationUser> GetByName(string name) 
        { 
            var result = _repository.GetByName(name);

            if (result == null) 
            {
                _result.SetFail("Brak użytkownika");
                return _result;
            }
            _result.SetSuccess(result);
            return _result;
        }
    }
}
