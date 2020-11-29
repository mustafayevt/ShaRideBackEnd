using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Response.CarBrand;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin")]
    public class CarBrandController : ControllerBase
    {
        private readonly ICarBrandService _carBrandService;

        public CarBrandController(ICarBrandService carBrandService)
        {
            _carBrandService = carBrandService;
        }

        /// <summary>
        /// Gets all carBrands
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCarBrands")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<CarBrandResponse>),200)]
        public async Task<IActionResult> GetCarBrands()
        {
            return Ok(await _carBrandService.GetCarBrands());
        }

        /// <summary>
        /// Gets specific carBrand by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCarBrandById/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CarBrandResponse),200)]
        public async Task<IActionResult> GetCarBrandById(int id)
        {
            return Ok(await _carBrandService.GetCarBrandById(id));
        }

        /// <summary>
        /// Inserts one carBrand.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCarBrand")]
        [ProducesResponseType(typeof(CarBrandResponse),201)]
        public async Task<IActionResult> InsertCarBrand(InsertCarBrandRequest request)
        {
            return Ok(await _carBrandService.InsertCarBrand(request));
        }

        /// <summary>
        /// Inserts multiple carBrands.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCarBrands")]
        [ProducesResponseType(typeof(ICollection<CarBrandResponse>),200)]
        public async Task<IActionResult> InsertCarBrands(ICollection<InsertCarBrandRequest> request)
        {
            return Ok(await _carBrandService.InsertCarBrands(request));
        }
        
        /// <summary>
        /// Updates carBrand.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateCarBrand")]        
        [ProducesResponseType(typeof(CarBrandResponse),200)]
        public async Task<IActionResult> UpdateCarBrand(UpdateCarBrandRequest request)
        {
            return Ok(await _carBrandService.UpdateCarBrand(request));
        }

        /// <summary>
        /// Deletes carBrand by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCarBrand/{id}")]
        public async Task<IActionResult> DeleteCarBrand(int id)
        {
            await _carBrandService.DeleteCarBrand(id);
            return Ok();
        }
    }
}