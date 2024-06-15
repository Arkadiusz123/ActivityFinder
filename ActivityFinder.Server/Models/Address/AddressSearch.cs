using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IAddressSearch
    {
        public Task<Result<Address>> GetAddressByOsmId(string osmId);
        public Task<Result<Address>> GetAddressByName(string name);
    }

    public class AddressSearch : IAddressSearch
    {
        private const string _nominatimOsmUlr = "https://nominatim.openstreetmap.org/";
        private readonly AppHttpClient _httpClient;
        private readonly Result<Address> _result;
        private readonly AppDbContext _context;

        public AddressSearch(AppDbContext context)
        {
            _httpClient = new AppHttpClient();
            _context = context;

            _result = new Result<Address>();
        }

        public async Task<Result<Address>> GetAddressByOsmId(string osmId)
        {
            var addressDb = _context.Addresses.SingleOrDefault(x => x.OsmId == osmId);
            if (addressDb != null)
            {
                _result.SetSuccess(addressDb);
                return _result;
            }

            var url = $"{_nominatimOsmUlr}lookup?format=json&accept-language=pl&addressdetails=1&osm_ids={osmId}";

            var addressessOsm = await GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.SetFail("Nie znaleziono adresu");
                return _result;
            }
            _result.SetSuccess(addressessOsm.First().ToAddress());
            return _result;
        }

        public async Task<Result<Address>> GetAddressByName(string name)
        {
            var url = $"{_nominatimOsmUlr}search?format=json&accept-language=pl&addressdetails=1&countrycodes=pl&q={name}";

            var addressessOsm = await GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                _result.SetFail("Nie znaleziono adresu");
                return _result;
            }

            _result.SetSuccess(addressessOsm.First().ToAddress());
            return _result;
        }

        private async Task<IEnumerable<AddressOsm>> GetResponse(string url)
        {
            return await _httpClient.GetResonse<List<AddressOsm>>(url);
        }
    }
}
