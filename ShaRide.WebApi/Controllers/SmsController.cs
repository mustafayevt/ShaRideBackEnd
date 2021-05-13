using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Sms;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost("SendSms")]
        public async Task<IActionResult> SendSms(SendSmsRequest request)
        {
            await _smsService.SendSms(request);
            return NoContent();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendSmsToAllPotentialClients(string body)
        {
            return Ok(await _smsService.SendSmsToAllPotentialClients(body));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendSmsToAllOurClients(string body)
        {
            return Ok(await _smsService.SendSmsToAllOurClients(body));
        }
    }
}