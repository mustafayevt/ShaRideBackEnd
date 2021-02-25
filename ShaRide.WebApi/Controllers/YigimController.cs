using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Response.Yigim;
using ShaRide.Application.Managers;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [YigimKey]
    public class YigimController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager _userManager;

        public YigimController(UserManager userManager, ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpGet("GetDebt")]
        public async Task<ActionResult> GetUserInfoById([Required]string userId)
        {
            var user = await _userManager.FindByUniqueKey(userId);

            if (user is null)
                throw new ApiException(
                    $"User with id : {userId} is not found out system. Please contact with administration");

            var debtOfUser = 0m;

            var response = new UserInfoResponse(user.Name, user.Surname, debtOfUser);

            return Ok(response);
        }
    }
}