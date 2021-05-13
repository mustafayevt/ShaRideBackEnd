using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoWrapper.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin,Basic")]
    public class CarController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarController(ICarService carService)
        {
            _carService = carService;
        }

        /// <summary>
        /// Gets all cars
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCars")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<CarResponse>),200)]
        public async Task<IActionResult> GetCars()
        {
            return Ok(await _carService.GetCarsAsync());
        }

        /// <summary>
        /// Gets specific car by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCarById/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CarResponse),200)]
        public async Task<IActionResult> GetCarById(int id)
        {
            return Ok(await _carService.GetCarByIdAsync(id));
        }

        /// <summary>
        /// Returns cars of user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetCarsByUserId/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ICollection<CarResponse>),200)]
        public async Task<IActionResult> GetCarsByUserIdAsync(int id)
        {
            return Ok(await _carService.GetCarsByUserIdAsync(id));
        }

        /// <summary>
        /// Inserts one car.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCar")]
        [ProducesResponseType(typeof(CarResponse),201)]
        public async Task<IActionResult> InsertCar(InsertCarRequest request)
        {
            return Ok(await _carService.InsertCarAsync(request));
        }

        /// <summary>
        /// Inserts multiple cars.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertCars")]
        [ProducesResponseType(typeof(ICollection<CarResponse>),200)]
        public async Task<IActionResult> InsertCars(ICollection<InsertCarRequest> request)
        {
            return Ok(await _carService.InsertCarsAsync(request));
        }

        /// <summary>
        /// Deletes car by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("DeleteCar/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _carService.DeleteCarAsync(id);
            return Ok();
        }
        
        [HttpGet("get-car-image")]
        [Produces(typeof(byte[]))]
        [AutoWrapIgnore]
        public async Task<IActionResult> GetCarImageByCarImageId([Required]int carImageId)
        {
            var image = await _carService.GetCarImageByCarImageId(carImageId);
            var data = image.Image;
            var filename = image.Id + image.Extension;
            return File(data, "application/force-download", filename);
        }
    }
}