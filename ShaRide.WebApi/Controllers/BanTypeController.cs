using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin")]
    public class BanTypeController : ControllerBase
    {
        private readonly IBanTypeService _banTypeService;

        public BanTypeController(IBanTypeService banTypeService)
        {
            _banTypeService = banTypeService;
        }

        /// <summary>
        /// Gets all banTypes
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBanTypes")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<BanTypeResponse>),200)]
        public async Task<IActionResult> GetBanTypes()
        {
            return Ok(await _banTypeService.GetBanTypes());
        }

        /// <summary>
        /// Gets specific banType by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetBanTypeById/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BanTypeResponse),200)]
        public async Task<IActionResult> GetBanTypeById(int id)
        {
            return Ok(await _banTypeService.GetBanTypeById(id));
        }

        /// <summary>
        /// Inserts one banType.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertBanType")]
        [ProducesResponseType(typeof(BanTypeResponse),200)]
        public async Task<IActionResult> InsertBanType(InsertBanTypeRequest request)
        {
            return Ok(await _banTypeService.InsertBanType(request));
        }

        /// <summary>
        /// Inserts multiple banTypes.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertBanTypes")]
        [ProducesResponseType(typeof(ICollection<BanTypeResponse>),200)]
        public async Task<IActionResult> InsertBanTypes(ICollection<InsertBanTypeRequest> request)
        {
            return Ok(await _banTypeService.InsertBanTypes(request));
        }
        
        /// <summary>
        /// Updates banType.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateBanType")]        
        [ProducesResponseType(typeof(BanTypeResponse),200)]
        public async Task<IActionResult> UpdateBanType(UpdateBanTypeRequest request)
        {
            return Ok(await _banTypeService.UpdateBanType(request));
        }

        /// <summary>
        /// Deletes banType by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteBanType/{id}")]
        public async Task<IActionResult> DeleteBanType(int id)
        {
            await _banTypeService.DeleteBanType(id);
            return Ok();
        }
    }
}