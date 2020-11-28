using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Gets all locations.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetLocations")]
        [Produces(typeof(ICollection<LocationResponse>))]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _locationService.GetLocations());
        }

        /// <summary>
        /// Gets all location points.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetLocationPoints")]
        [Produces(typeof(LocationResponse))]
        public async Task<IActionResult> GetLocationPoints()
        {
            return Ok(await _locationService.GetLocationPoints());
        }

        /// <summary>
        /// Gets location points by location id.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetLocationPointsByLocationId/{locationId}")]
        [Produces(typeof(ICollection<LocationResponse>))]
        public async Task<IActionResult> GetLocationPointsByLocationId(int locationId)
        {
            return Ok(await _locationService.GetLocationPointsByLocationId(locationId));
        }

        /// <summary>
        /// Gets location by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetLocationById/{id}")]
        [Produces(typeof(LocationResponse))]
        public async Task<IActionResult> GetLocationById(int id)
        {
            return Ok(await _locationService.GetLocationById(id));
        }

        /// <summary>
        /// Inserts location.
        /// </summary>
        /// <param name="locationRequest"></param>
        /// <returns></returns>
        [HttpPost("InsertLocation")]
        [Produces(typeof(LocationResponse))]
        public async Task<IActionResult> InsertLocation(InsertLocationRequest locationRequest)
        {
            return Ok(await _locationService.InsertLocation(locationRequest));
        }

        /// <summary>
        /// Inserts location point.
        /// </summary>
        /// <param name="locationPointRequest"></param>
        /// <returns></returns>
        [HttpPost("InsertLocationPoint")]
        [Produces(typeof(LocationPointResponse))]
        public async Task<IActionResult> InsertLocationPoint(InsertLocationPointRequest locationPointRequest)
        {
            return Ok(await _locationService.InsertLocationPoint(locationPointRequest));
        }

        /// <summary>
        /// Updates location.
        /// </summary>
        /// <param name="locationRequest"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocation")]
        [Produces(typeof(LocationResponse))]
        public async Task<IActionResult> UpdateLocation(UpdateLocationRequest locationRequest)
        {
            return Ok(await _locationService.UpdateLocation(locationRequest));
        }

        /// <summary>
        /// Updates location point.
        /// </summary>
        /// <param name="locationPointRequest"></param>
        /// <returns></returns>
        [HttpPut("UpdateLocationPoint")]
        [Produces(typeof(LocationPointResponse))]
        public async Task<IActionResult> UpdateLocationPoint(UpdateLocationPointRequest locationPointRequest)
        {
            return Ok(await _locationService.UpdateLocationPoint(locationPointRequest));
        }

        /// <summary>
        /// Deletes location.
        /// </summary>
        /// <param name="locationId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteLocation/{locationId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteLocation(int locationId)
        {
            await _locationService.DeleteLocation(locationId);

            return Ok();
        }

        /// <summary>
        /// Deletes location point by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteLocationPoint/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> DeleteLocationPoint(int id)
        {
            await _locationService.DeleteLocationPoint(id);

            return Ok();
        }
    }
}