using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("GetLocations")]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _locationService.GetLocations());
        }

        [HttpGet("GetLocationPoints")]
        public async Task<IActionResult> GetLocationPoints()
        {
            return Ok(await _locationService.GetLocationPoints());
        }

        [HttpGet("GetLocationPointsByLocationId/{locationId}")]
        public async Task<IActionResult> GetLocationPointsByLocationId(int locationId)
        {
            return Ok(await _locationService.GetLocationPointsByLocationId(locationId));
        }

        [HttpGet("GetLocationById/{id}")]
        public async Task<IActionResult> GetLocationById(int id)
        {
            return Ok(await _locationService.GetLocationById(id));
        }

        [HttpPost("InsertLocation")]
        public async Task<IActionResult> InsertLocation(InsertLocationRequest locationRequest)
        {
            return Ok(await _locationService.InsertLocation(locationRequest));
        }

        [HttpPost("InsertLocationPoint")]
        public async Task<IActionResult> InsertLocationPoint(InsertLocationPointRequest locationPointRequest)
        {
            return Ok(await _locationService.InsertLocationPoint(locationPointRequest));
        }

        [HttpPut("UpdateLocation")]
        public async Task<IActionResult> UpdateLocation(UpdateLocationRequest locationRequest)
        {
            return Ok(await _locationService.UpdateLocation(locationRequest));
        }

        [HttpPut("UpdateLocationPoint")]
        public async Task<IActionResult> UpdateLocationPoint(UpdateLocationPointRequest locationPointRequest)
        {
            return Ok(await _locationService.UpdateLocationPoint(locationPointRequest));
        }

        [HttpDelete("DeleteLocation/{locationId}")]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            await _locationService.DeleteLocation(locationId);

            return Ok();
        }

        [HttpDelete("DeleteLocationPoint/{id}")]
        public async Task<IActionResult> DeleteLocationPoint(int id)
        {
            await _locationService.DeleteLocationPoint(id);

            return Ok();
        }
    }
}