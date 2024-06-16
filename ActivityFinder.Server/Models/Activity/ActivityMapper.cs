namespace ActivityFinder.Server.Models
{
    public class ActivityMapper : IEntityToVmMapper<Activity, ActivityVmWrapper>
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

        public ActivityVmWrapper MapListToVm(IQueryable<Activity> queryForPage, int totalCount, string userName)
        {
            var vmList = queryForPage.Select(x => new
            {
                x.ActivityId,
                x.Title,
                x.Date,
                x.Address.Town,
                x.Address.Name,
                x.Address.Road,
                x.Address.HouseNumber,
                CreatedByUser = x.Creator.UserName == userName
            })
                .AsEnumerable()
                .Select(x => new ActivityVm
                {
                    Id = x.ActivityId,
                    Title = x.Title,
                    Address = Address.ShortString(x.Name, x.Town, x.Road, x.HouseNumber),
                    Date = x.Date.ToString(ConstValues.DateFormatWithHour),
                    CreatedByUser = x.CreatedByUser
                }).ToList();

            return new ActivityVmWrapper() 
            {
                Data = vmList,
                TotalCount = totalCount
            };
        }
    }
}
