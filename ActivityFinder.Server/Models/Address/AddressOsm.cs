﻿using System.Text.Json.Serialization;

namespace ActivityFinder.Server.Models
{
    public class AddressOsm
    {
        [JsonPropertyName("lat")]
        public required string Lat { get; set; }

        [JsonPropertyName("lon")]
        public required string Lon { get; set; }

        [JsonPropertyName("address")]
        public required AddressOsmDetails Details { get; set; }

        [JsonPropertyName("display_name")]
        public required string DisplayName { get; set; }

        [JsonPropertyName("addresstype")]
        public required string Type { get; set; }

        [JsonPropertyName("osm_id")]
        public double OsmId { get; set; }

        [JsonPropertyName("osm_type")]
        public required string OsmType { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class AddressOsmDetails
    {
        [JsonPropertyName("house_number")]
        public string? HouseNumber { get; set; }

        [JsonPropertyName("road")]
        public string? Road { get; set; }

        [JsonPropertyName("city")]
        public string? City { get; set; }   //one of them(city,town,village) required, save as city

        [JsonPropertyName("town")]
        public string? Town { get; set; }

        [JsonPropertyName("village")]
        public string? Village { get; set; }

        [JsonPropertyName("postcode")]
        public required string Postcode { get; set; }

        [JsonPropertyName("county")]
        public string? County { get; set; }     //(powiat), for cities empty, then save city parameter

        [JsonPropertyName("state")]
        public required string State { get; set; }  //(województwo)

        [JsonPropertyName("country")]
        public required string Country { get; set; }
    }
}
