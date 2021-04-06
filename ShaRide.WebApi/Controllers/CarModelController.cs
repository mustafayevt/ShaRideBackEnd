using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Response.CarModel;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin")]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelService _carModelService;

        public CarModelController(ICarModelService carModelService)
        {
            _carModelService = carModelService;
        }

        /// <summary>
        /// Gets all carModels
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCarModels")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<CarModelResponse>),200)]
        public async Task<IActionResult> GetCarModels()
        {
            return Ok(await _carModelService.GetCarModelsAsync());
        }

        /// <summary>
        /// Gets specific carModel by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCarModelById/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CarModelResponse),200)]
        public async Task<IActionResult> GetCarModelById(int id)
        {
            return Ok(await _carModelService.GetCarModelByIdAsync(id));
        }

        /// <summary>
        /// Inserts one carModel.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCarModel")]
        [ProducesResponseType(typeof(CarModelResponse),201)]
        public async Task<IActionResult> InsertCarModel(InsertCarModelRequest request)
        {
            return Ok(await _carModelService.InsertCarModelAsync(request));
        }

        /// <summary>
        /// Inserts multiple carModels.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCarModels")]
        [ProducesResponseType(typeof(ICollection<CarModelResponse>),200)]
        public async Task<IActionResult> InsertCarModels(ICollection<InsertCarModelRequest> request)
        {
            return Ok(await _carModelService.InsertCarModelsAsync(request));
        }
        
        /// <summary>
        /// Updates carModel.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateCarModel")]        
        [ProducesResponseType(typeof(CarModelResponse),200)]
        public async Task<IActionResult> UpdateCarModel(UpdateCarModelRequest request)
        {
            return Ok(await _carModelService.UpdateCarModelAsync(request));
        }

        /// <summary>
        /// Updates banId in car models.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("UpdateCarModelBanId")]
        public async Task<IActionResult> UpdateCarModelBanId(ICollection<UpdateCarModelBanIdRequest> request)
        {
            return Ok(await _carModelService.UpdateCarModelBanIdAsync(request));
        }

        /// <summary>
        /// Deletes carModel by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCarModel/{id}")]
        public async Task<IActionResult> DeleteCarModel(int id)
        {
            await _carModelService.DeleteCarModelAsync(id);
            return Ok();
        }
    }
}