using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IUserService
    {
        public ValueResult<ApplicationUser> GetByName(string name);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(AppDbContext context)
        {
            _repository = new UserRepository(context);
        }

        public ValueResult<ApplicationUser> GetByName(string name) 
        { 
            var result = _repository.GetByName(name);

            if (result == null) 
            {
                return new ValueResult<ApplicationUser>(false, "Brak użytkownika");
            }

            return new ValueResult<ApplicationUser>(result, true);
        }
    }
}
