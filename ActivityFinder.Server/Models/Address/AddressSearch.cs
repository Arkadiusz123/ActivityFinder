using ActivityFinder.Server.Database;
using ActivityFinder.Server.OtherTools.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ActivityFinder.Server.Models
{
    public interface IAddressSearch
    {
        public Result<Address> GetAddressByOsmId(string osmId);
        public Result<Address> GetAddressByName(string name);
    }

    public class AddressSearch : IAddressSearch
    {
        private const string _nominatimOsmUlr = "https://nominatim.openstreetmap.org/";
        private readonly HttpClient _httpClient;
        private readonly Result<Address> _result;
        private readonly AppDbContext _context;

        public AddressSearch(AppDbContext context)
        {
            _httpClient = new HttpClient();
            _httpClient.SetBasicHeaders();
            _context = context;

            _result = new Result<Address>();
        }

        public Result<Address> GetAddressByOsmId(string osmId)
        {
            var addressDb = _context.Addresses.SingleOrDefault(x => x.OsmId == osmId);
            if (addressDb != null)
            {
                _result.SetSuccess(addressDb);
                return _result;
            }

            var url = $"{_nominatimOsmUlr}lookup?format=json&accept-language=pl&addressdetails=1&osm_ids={osmId}";

            var addressessOsm = GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.SetFail("Nie znaleziono adresu");
                return _result;
            }
            _result.SetSuccess(addressessOsm.First().ToAddress());
            return _result;
        }

        public Result<Address> GetAddressByName(string name)
        {
            var url = $"{_nominatimOsmUlr}search?format=json&accept-language=pl&addressdetails=1&countrycodes=pl&q={name}";

            var addressessOsm = GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.SetFail("Nie znaleziono adresu");
                return _result;
            }

            _result.SetSuccess(addressessOsm.First().ToAddress());
            return _result;
        }

        private IEnumerable<AddressOsm> GetResponse(string url)
        {
            var response = _httpClient.GetStringAsync(url).Result;
            return JsonSerializer.Deserialize<List<AddressOsm>>(response.ToString()) ?? new List<AddressOsm>();
        }
    }
}
