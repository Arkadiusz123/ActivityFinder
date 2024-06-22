namespace ActivityFinder.Server.Models.Shared
{
    public class SinglePageData<T>
    {
        public SinglePageData(int totalCount, IEnumerable<T> data)
        {
            TotalCount = totalCount;
            Data = data;
        }

        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
