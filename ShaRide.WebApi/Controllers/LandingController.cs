using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LandingController : Controller
    {
        private readonly IAccountService _accountService;

        public LandingController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("")]
        [HttpGet("Home")]
        [HttpGet("Landing")]
        // GET
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost("InsertPotentialClient")]
        public async Task<IActionResult> InsertPotentialClient(InsertPotentialClientPhoneRequest request)
        {
            await Task.Delay(1500); // to show spinner to client.

            return Ok(await _accountService.InsertPotentialClientPhone(request));
        }
    }
}