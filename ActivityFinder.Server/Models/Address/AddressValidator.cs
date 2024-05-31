using ActivityFinder.Server.OtherTools.Extensions;
using System.Text.Json;

namespace ActivityFinder.Server.Models
{
    public class AddressValidator
    {
        private const string _nominatimOsmUlr = "https://nominatim.openstreetmap.org/";

        public Result<Address> GetAddress(AddressDTO addressDto)
        {
            //TODO: find in db and return if exists


            HttpClient client = new HttpClient();
            client.SetBasicHeaders();

            var url = $"{_nominatimOsmUlr}search?format=json&accept-language=pl&addressdetails=1&" + 
                $"street={addressDto.Road} {addressDto.HouseNumber}&city={addressDto.City}&state={addressDto.State}&postalcode={addressDto.Postcode}";

            var response = client.GetStringAsync(url).Result;
            var addressessOsm = JsonSerializer.Deserialize<List<AddressOsm>>(response.ToString());

            var result = new Result<Address>();

            if (addressessOsm == null)
            {
                result.Message = "Nie znaleziono adresu";
                return result;
            }

            //sometimes there are multiple addressess with the same data, differs only by category
            if (addressessOsm.GroupBy(x => x.DisplayName).Count() > 1) 
            {
                result.Message = "Znaleziono wiele adresów. Uzupełnij więcej informacji";
                return result;
            }

            var addressOsm = addressessOsm.First();
            result.Value = addressessOsm;   //TODO: map addressOsm to Address
            return result;
        }
    }
}
