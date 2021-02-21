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
    // [ApiKey]
    // [Authorize(Roles = "Admin")] //TODO: uncomment these two lines before prod.
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
    }
}