using ActivityFinder.Server.Database;

namespace ActivityFinder.Server.Models
{
    public interface IAddressSearch
    {
        public Task<ValueResult<Address>> GetAddressByOsmId(string osmId);
        public Task<ValueResult<Address>> GetAddressByName(string name);
    }

    public class AddressSearch : IAddressSearch
    {
        private const string _nominatimOsmUlr = "https://nominatim.openstreetmap.org/";
        private readonly AppHttpClient _httpClient;
        private readonly AppDbContext _context;

        public AddressSearch(AppDbContext context)
        {
            _httpClient = new AppHttpClient();
            _context = context;
        }

        public async Task<ValueResult<Address>> GetAddressByOsmId(string osmId) //todo usunac try catch
        {
            var addressDb = _context.Addresses.SingleOrDefault(x => x.OsmId == osmId);
            if (addressDb != null)
            {
                return new ValueResult<Address>(addressDb, true);
            }

            var url = $"{_nominatimOsmUlr}lookup?format=json&accept-language=pl&addressdetails=1&osm_ids={osmId}";

            var addressessOsm = await GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                return new ValueResult<Address>(false, "Nie znaleziono adresu");
            }
            return new ValueResult<Address>(addressessOsm.First().ToAddress(), true);
        }

        public async Task<ValueResult<Address>> GetAddressByName(string name)
        {
            var url = $"{_nominatimOsmUlr}search?format=json&accept-language=pl&addressdetails=1&countrycodes=pl&q={name}";

            var addressessOsm = await GetResponse(url);

            if (addressessOsm == null || addressessOsm.Count() == 0)
            {
                return new ValueResult<Address>(false, "Nie znaleziono adresu");
            }

            return new ValueResult<Address>(addressessOsm.First().ToAddress(), true);
        }

        private async Task<IEnumerable<AddressOsm>> GetResponse(string url)
        {
            return await _httpClient.GetResonse<List<AddressOsm>>(url);
        }
    }
}
