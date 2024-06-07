﻿using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressSearch _addressSearch;
        private readonly AppDbContext _context;

        public AddressController(ILogger<AddressController> logger, AppDbContext context)
        {
            _context = context;
            _logger = logger;
            _addressSearch = new AddressSearch(_context);
        }

        [HttpGet]
        [Route("{input}")]
        public IActionResult GetBySearchInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return BadRequest("Nie podano adresu");

            var result = _addressSearch.GetAddressByName(input);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Value!.ToAddressDto());
        }
    }
}
