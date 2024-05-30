﻿namespace ActivityFinder.Server.DTOs
{
    public class AddressDTO
    {
        public required string HouseNumber { get; set; }
        public required string Road { get; set; }
        public required string State { get; set; }
        public required string Postcode { get; set; }
    }
}
