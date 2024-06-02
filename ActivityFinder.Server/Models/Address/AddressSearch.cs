using ActivityFinder.Server.OtherTools.Extensions;
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

        public AddressSearch()
        {
            _httpClient = new HttpClient();
            _httpClient.SetBasicHeaders();

            _result = new Result<Address>();
        }

        public Result<Address> GetAddressByOsmId(string osmId)
        {
            //TODO: find in db and return if exists

            var url = $"{_nominatimOsmUlr}lookup?format=json&accept-language=pl&addressdetails=1&osm_ids={osmId}";

            var addressessOsm = GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.Message = "Nie znaleziono adresu";
                return _result;
            }

            _result.Value = addressessOsm.First().ToAddress();
            _result.Success = true;
            return _result;
        }

        public Result<Address> GetAddressByName(string name)
        {
            var url = $"{_nominatimOsmUlr}search?format=json&accept-language=pl&addressdetails=1&q={name}";

            var addressessOsm = GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.Message = "Nie znaleziono adresu";
                return _result;
            }

            _result.Value = addressessOsm.First().ToAddress();
            _result.Success = true;
            return _result;
        }

        private IEnumerable<AddressOsm> GetResponse(string url)
        {
            var response = _httpClient.GetStringAsync(url).Result;
            return JsonSerializer.Deserialize<List<AddressOsm>>(response.ToString()) ?? new List<AddressOsm>();
        }
    }
}
