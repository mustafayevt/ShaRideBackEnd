using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Management;
using ShaRide.Application.Services.Interface;
using System.Threading.Tasks;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ManagementController : Controller
    {
        private readonly IUserService _userService;

        public ManagementController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AllUsers")]
        public async Task<IActionResult> AllUsers(UserFilterRequest request)
        {
            var result = await _userService.AllUsers(request);
            return Ok(result);
        }
    }
}