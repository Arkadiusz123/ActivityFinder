using Microsoft.EntityFrameworkCore;

namespace ActivityFinder.Server.Models
{
    [Index(nameof(OsmId), IsUnique = true)]
    public class Address
    {
        public int AddressId { get; set; }
        public required string OsmId { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string? Name { get; set; }
        public required string Type { get; set; }
        public string? HouseNumber { get; set; }
        public string? Road { get; set; }
        public required string Town { get; set; }   //city, town or village
        public required string Postcode { get; set; }
        public required string County { get; set; }     //(pl: powiat), for cities empty, then save city parameter
        public required string State { get; set; }  //pl: województwo
        public required string Country { get; set; }

        public List<Activity> Activities { get; set; } = [];

        public override string ToString()
        {
            List<string> list = [];

            if (!string.IsNullOrEmpty(Name))
                list.Add(Name);

            list.Add(Town);

            if(!string.IsNullOrEmpty(Road))
                list.Add($"ul. {Road}{(!string.IsNullOrEmpty(HouseNumber) ? (" " + HouseNumber) : "")}");

            list.Add($"powiat {County}");
            list.Add($"województwo {State}");

            return string.Join(", ", list);
        }

        public static string ShortString(string name, string town, string road, string number)
        {
            List<string> list = [town];           

            if (!string.IsNullOrEmpty(road))
                list.Add($"ul. {road}{(!string.IsNullOrEmpty(number) ? (" " + number) : "")}");

            if (!string.IsNullOrEmpty(name))
                list.Add(name);

            return string.Join(", ", list);
        }    
    }
}
