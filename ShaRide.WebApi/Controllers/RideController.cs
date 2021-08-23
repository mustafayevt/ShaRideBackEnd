using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Application.Pagination;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Basic")]
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
        public async Task<IActionResult> GetActiveRides(GetActiveRidesRequest request)
        {
            return Ok(await _rideService.GetActiveRides(request));
        }

        [HttpPost("GetRides")]
        [AllowAnonymous]
        [ProducesResponseType(200, Type = typeof(PaginatedList<RideResponse>))]
        [Produces("application/json")]
        public async Task<IActionResult> GetRides(RidesFilterRequest ridesFilterRequest)
        {
            var rides = await _rideService.GetRides(ridesFilterRequest);
            return Ok(rides);
        }

        [HttpPost("UpdateRideState")]
        public async Task<IActionResult> UpdateRideState(UpdateRideStateRequest request)
        {
            return Ok(await _rideService.UpdateRideState(request));
        }

        [HttpPost("GiveFeedbackForRide")]
        public async Task<IActionResult> GiveFeedbackForRide(RideFeedbackRequest request)
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

        [HttpGet("GetCurrentUserRidesAsDriver")]
        public async Task<IActionResult> GetCurrentUsersRidesAsDriver()
        {
            return Ok(await _rideService.GetCurrentUsersRidesAsDriver());
        }

        [HttpGet("GetCurrentUserRidesAsPassenger")]
        public async Task<IActionResult> GetCurrentUsersRidesAsPassenger()
        {
            return Ok(await _rideService.GetCurrentUserRidesAsPassenger());
        }

        [HttpGet("GetCurrentUserRideRequests")]
        public async Task<IActionResult> GetCurrentUsersRideRequests()
        {
            return Ok(await _rideService.GetCurrentUsersRideRequests());
        }

        [HttpPost("CancelRide")]
        public async Task<IActionResult> CancelRide(CancelRideRequest request)
        {
            return Ok(await _rideService.CancelRide(request));
        }

        [HttpPost("CancelPassengerRideRequest/{rideId}")]
        public async Task<IActionResult> CancelPassengerRideRequest(int rideId)
        {
            return Ok(await _rideService.CancelPassengerRideRequest(rideId));
        }

        [HttpGet("SuggestRidesToUser")]
        public async Task<IActionResult> SuggestRidesToUser()
        {
            return Ok(await _rideService.SuggestRidesToUser());
        }

        [HttpGet("SuggestRidesToUserBasedOnUserFavoriteRoute")]
        public async Task<IActionResult> SuggestRidesToUserBasedOnUserFavoriteRoute()
        {
            return Ok(await _rideService.SuggestRidesToUserBasedOnUserFavoriteRoute());
        }

        [HttpPut("DeactivateUserRideRequest/{requestId:int}")]
        public async Task<IActionResult> DeactivateUserRideRequest(int requestId)
        {
            return Ok(await _rideService.DeactivateUserRequest(requestId));
        }
    }
}