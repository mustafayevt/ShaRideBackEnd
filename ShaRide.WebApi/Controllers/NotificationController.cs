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
    [Authorize(Roles = "Admin,Basic")]
    public class NotificationController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public NotificationController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Inserts new messages.
        /// </summary>
        /// <remarks>no need to send sender id. senderId assigns from auth user JWT token</remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        //[HttpPost("SendNotification")]
        //public async Task<IActionResult> SendNotification(InsertMessageRequest request)
        //{
        //    return Ok(await _messageService.(request));
        //}
    }
}