using ActivityFinder.Server.Models;

namespace ActivityFinderTests
{
    public class TestObjectFactory
    {
        public static Address CreateAddress()
        {
            return new Address
            {
                OsmId = "w1284375894",
                Type = "test",
                Country = "Poland",
                County = "Krakow",
                Town = "Krakow",
                State = "małopolskie",
                Postcode = "80-080"
            };
        }

        public static Activity CreateActivity(int? id = null)
        {
            return new Activity
            {
                ActivityId = id ?? 0,
                Title = "Tutuł",
                Address = CreateAddress(),
                Creator = null,
                Date = DateTime.Now,
                Description = "description"
            };
        }

    }
}
