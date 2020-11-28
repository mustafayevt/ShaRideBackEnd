using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin")]
    public class RestrictionController : ControllerBase
    {
        private readonly IRestrictionService _restrictionService;

        public RestrictionController(IRestrictionService restrictionService)
        {
            _restrictionService = restrictionService;
        }

        /// <summary>
        /// Gets all restrictions
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetRestrictions")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<RestrictionResponse>),200)]
        public async Task<IActionResult> GetRestrictions()
        {
            return Ok(await _restrictionService.GetRestrictions());
        }

        /// <summary>
        /// Gets specific restriction by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetRestrictionById/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(RestrictionResponse),200)]
        public async Task<IActionResult> GetRestrictionById(int id)
        {
            return Ok(await _restrictionService.GetRestrictionById(id));
        }

        /// <summary>
        /// Inserts one restriction.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertRestriction")]
        [ProducesResponseType(typeof(RestrictionResponse),200)]
        public async Task<IActionResult> InsertRestriction(InsertRestrictionRequest request)
        {
            return Ok(await _restrictionService.InsertRestriction(request));
        }

        /// <summary>
        /// Inserts multiple restrictions.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertRestrictions")]
        [ProducesResponseType(typeof(ICollection<RestrictionResponse>),200)]
        public async Task<IActionResult> InsertRestrictions(ICollection<InsertRestrictionRequest> request)
        {
            return Ok(await _restrictionService.InsertRestrictions(request));
        }
        
        /// <summary>
        /// Updates restriction.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateRestriction")]        
        [ProducesResponseType(typeof(RestrictionResponse),200)]
        public async Task<IActionResult> UpdateRestriction(UpdateRestrictionRequest request)
        {
            return Ok(await _restrictionService.UpdateRestriction(request));
        }

        /// <summary>
        /// Deletes restriction by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteRestriction/{id}")]
        public async Task<IActionResult> DeleteRestriction(int id)
        {
            await _restrictionService.DeleteRestriction(id);
            return Ok();
        }
    }
}