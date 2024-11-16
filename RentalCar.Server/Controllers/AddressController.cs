using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalCar.Entity.Entities;
using RentalCar.Model;

namespace RentalCar.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressCityService _addressCityService;
        private readonly IAddressDistrictService _addressDistrictService;
        private readonly IAddressWardService _addressWardService;

        public AddressController(IAddressCityService AddressCityService, IAddressDistrictService AddressDistrict, IAddressWardService AddressWard)
        {
            _addressCityService = AddressCityService;
            _addressDistrictService = AddressDistrict;
            _addressWardService = AddressWard;
        }

        // GET: api/Address
        [AllowAnonymous]
        [HttpGet("GetAddressCitys")]
        public async Task<ActionResult<IEnumerable<AddressCity>>> GetAddressCitys()
        {
            var addressCity = await _addressCityService.GetAllAddressCityAsync();
            if (addressCity == null)
                return NotFound(new { message = "Address not found" });
            return Ok(new { AddressCity = addressCity });
        }

        // GET: api/Address/{id}
        [AllowAnonymous]
        [HttpGet("GetAllAddressDistrictOfCity")]
        public async Task<ActionResult<AddressDistrict>> GetAllAddressDistrictOfCity(Guid id)
        {
            var addressDistrict = await _addressDistrictService.GetAllAddressDistrictOfCityAsync(id);
            if (addressDistrict == null)
            {
                return NotFound(new { message = "AddressDistrict not found" });
            }

            return Ok(new { AddressDistrict = addressDistrict });
        }

        [AllowAnonymous]
        [HttpGet("GetAllAddressWardOfDistrict")]
        public async Task<ActionResult<AddressWard>> GetAllAddressWardOfDistrict(Guid id)
        {
            var addressWard = await _addressWardService.GetAllAddressWardOfDistrictAsync(id);
            if (addressWard == null)
            {
                return NotFound(new { message = "AddressWard not found" });
            }

            return Ok(new { AddressWard = addressWard });
        }
    }
}
