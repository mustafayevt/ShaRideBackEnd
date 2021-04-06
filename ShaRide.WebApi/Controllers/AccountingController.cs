using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize]
    public class AccountingController : ControllerBase
    {
        private readonly IAccountingService _accountingService;

        public AccountingController(IAccountingService accountingService)
        {
            _accountingService = accountingService;
        }

        [HttpGet("GetCarAccounting")]
        public async Task<IActionResult> GetCarAccounting()
        {
            return Ok(await _accountingService.GetCarAccounting());
        }

        [HttpGet("GetPaymentDetails")]
        public async Task<IActionResult> GetPaymentDetails()
        {
            return Ok(await _accountingService.GetPaymentDetails());
        }
    }
}