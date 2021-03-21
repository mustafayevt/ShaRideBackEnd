using System.Collections.Generic;
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

        [HttpGet("GetPassengerToRideRequests")]
        public async Task<IActionResult> GetPassengerToRideRequests()
        {
            return Ok(await _rideService.GetPassengerToRideRequests());
        }

        [HttpPost("RespondUserRideRequest")]
        public async Task<IActionResult> RespondUserRideRequest(List<DriverRespondRequest> requests)
        {
            return Ok(await _rideService.RespondUserRideRequest(requests));
        }

        [HttpGet("GetCurrentUsersRidesAsDriver")]
        public async Task<IActionResult> GetCurrentUsersRidesAsDriver()
        {
            return Ok(await _rideService.GetCurrentUsersRidesAsDriver());
        }

        [HttpGet("GetCurrentUsersRidesAsPassenger")]
        public async Task<IActionResult> GetCurrentUsersRidesAsPassenger()
        {
            return Ok(await _rideService.GetCurrentUsersRidesAsPassenger());
        }
    }
}