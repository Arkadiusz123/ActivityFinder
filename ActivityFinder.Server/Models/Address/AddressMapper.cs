using System.Globalization;

namespace ActivityFinder.Server.Models
{
    public static class AddressMapper
    {
        public static Address ToAddress(this AddressOsm addressOsm)
        {
            var town = "";

            if (!string.IsNullOrEmpty(addressOsm.Details.City))
                town = addressOsm.Details.City;
            else if (!string.IsNullOrEmpty(addressOsm.Details.Town))
                town = addressOsm.Details.Town;
            else
                town = addressOsm.Details.Village;

            var county = !string.IsNullOrEmpty(addressOsm.Details.County) ? addressOsm.Details.County : addressOsm.Details.City;

            var address = new Address()
            {
                OsmId = addressOsm.OsmType[0] + addressOsm.OsmId.ToString(),
                Type = addressOsm.Type,
                Lat = Convert.ToDouble(addressOsm.Lat, CultureInfo.InvariantCulture),
                Lon = Convert.ToDouble(addressOsm.Lon, CultureInfo.InvariantCulture),
                HouseNumber = addressOsm.Details.HouseNumber,
                Road = addressOsm.Details.Road,
                Town = town ?? "",
                Postcode = addressOsm.Details.Postcode,
                County = (county ?? "").Replace("powiat ", ""),
                State = addressOsm.Details.State.Replace("województwo ", ""),
                Country = addressOsm.Details.Country,
                Name = addressOsm.Name
            };
            return address;
        }

        public static AddressDTO ToAddressDto(this Address address)
        {
            var dto = new AddressDTO()
            {
                OsmId = address.OsmId,
                DisplayName = address.ToString()
            };
            return dto;
        }
    }
}
