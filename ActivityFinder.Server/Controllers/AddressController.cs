using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace ActivityFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly IAddressSearch _addressSearch;

        public AddressController(ILogger<AddressController> logger)
        {
            _logger = logger;
            _addressSearch = new AddressSearch();
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
