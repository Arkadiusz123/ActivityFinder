using System.Linq.Expressions;

namespace ActivityFinder.Server.Models
{
    public class ActivityMapper
    {
        public static Activity ToActivity(ActivityDTO activityDTO, Address address, ApplicationUser user)
        {
            var activity = new Activity()
            {
                Title = activityDTO.Title,
                ActivityId = activityDTO.Id ?? 0,
                Description = activityDTO.Description!,
                Date = activityDTO.Date!.Value,
                OtherInfo = activityDTO.OtherInfo,
                Address = address,
                Creator = user,
                UsersLimit = activityDTO.UsersLimit
            };
            return activity;
        }

        public static ActivityDTO ToDTO(Activity activity)
        {
            var dto = new ActivityDTO()
            {
                Id = activity.ActivityId,
                Title = activity.Title,
                Address = activity.Address.ToAddressDto(),
                Description = activity.Description,
                Date = activity.Date,
                OtherInfo = activity.OtherInfo,
                UsersLimit = activity.UsersLimit
            };
            return dto;
        }

        public static Expression<Func<Activity, ActivityVm>> SelectVmExpression(string userName)
        {
            return x => new ActivityVm
            {
                Id = x.ActivityId,
                Title = x.Title,
                Address = Address.ShortString(x.Address.Name, x.Address.Town, x.Address.Road, x.Address.HouseNumber),
                Date = x.Date.ToString(ConstValues.DateFormatWithHour),
                CreatedByUser = x.Creator.UserName == userName,
                JoinedUsers = x.JoinedUsers.Count,
                UsersLimit = x.UsersLimit,
                AlreadyJoined = x.JoinedUsers.Any(y => y.UserName == userName),
                Description = x.Description
            };
        }        
    }
}
