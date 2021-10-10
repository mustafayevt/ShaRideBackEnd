using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Message;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    //[Authorize(Roles = "Admin,Basic")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Inserts new messages.
        /// </summary>
        /// <remarks>no need to send sender id. senderId assigns from auth user JWT token</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("InsertMessage")]
        public async Task<IActionResult> InsertMessage(InsertMessageRequest request)
        {
            return Ok(await _messageService.InsertMessage(request));
        }

        [HttpGet("GetCurrentUserMessageGroups")]
        public async Task<IActionResult> GetCurrentUserMessageGroups()
        {
            return Ok(await _messageService.GetCurrentUserMessageGroups());
        }

        [HttpGet("GetCurrentUserMessageGroup/{rideId}")]
        public async Task<IActionResult> GetCurrentUserMessageGroup(int rideId)
        {
            return Ok(await _messageService.GetCurrentUserMessageGroup(rideId));
        }
    }
}