using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [ApiKey]
    public class RideController : ControllerBase
    {
        private readonly IRideService _rideService;

        public RideController(IRideService rideService)
        {
            _rideService = rideService;
        }

        [HttpPost("InsertRide")]
        public async Task<IActionResult> InsertRide(InsertRideRequest request)
        {
            return Ok(await _rideService.InsertRide(request));
        }

        [HttpPost("GetActiveRides")]
        [AllowAnonymous]
        public IActionResult GetActiveRides(GetActiveRidesRequest request)
        {
            return Ok(_rideService.GetActiveRides(request));
        }

        [HttpPost("UpdateRideState")]
        public async Task<IActionResult> UpdateRideState(UpdateRideStateRequest request)
        {
            return Ok(await _rideService.UpdateRideState(request));
        }

        [HttpPost("AddPassengerToRide")]
        public async Task<IActionResult> AddPassengerToRide(AddPassengerToRideRequest request)
        {
            return Ok(await _rideService.AddPassengerToRide(request));
        }
    }
}