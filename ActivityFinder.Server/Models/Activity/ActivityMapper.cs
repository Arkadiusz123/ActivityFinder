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
                Creator = user               
            };
            return activity;
        }

        public ActivityVmWrapper MapListToVm(IQueryable<Activity> queryForPage, int totalCount)
        {
            var vmList = queryForPage.Select(x => new ActivityVm
            {
                Id = x.ActivityId,
                Title = x.Title,
                Address = x.Address.ToString(),
                Date = x.Date.ToString(ConstValues.DateFormatWithHour),
            }).ToList();

            return new ActivityVmWrapper() 
            {
                Data = vmList,
                TotalCount = totalCount
            };
        }
    }
}
