using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Invoice;
using ShaRide.Application.DTO.Response.Yigim;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [YigimKey]
    public class YigimController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager _userManager;
        private readonly IInvoiceService _invoiceService;

        public YigimController(UserManager userManager, ApplicationDbContext applicationDbContext, IInvoiceService invoiceService)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
            _invoiceService = invoiceService;
        }

        [HttpGet("GetUserBalance")] 
        public async Task<ActionResult> GetUserInfoById([Required]string userId)
        {
            var user = await _userManager.FindByUniqueKey(userId);

            if (user is null)
                throw new ApiException(
                    $"User with id : {userId} is not found out system. Please contact with administration");

            var response = new UserInfoResponse(user.Name, user.Surname, user.Balance);

            return Ok(response);
        }

        [HttpPost("RegisterPayment")]
        public async Task<ActionResult> RegisterPayment(RegisterInvoiceRequest request)
        {
            return Ok(await _invoiceService.RegisterInvoice(request));
        }

        [HttpGet("GetPaymentDetails")]
        public async Task<ActionResult> GetPaymentDetails([Required] string invoiceNumber)
        {
            return Ok(await _invoiceService.GetInvoiceDetail(invoiceNumber));
        }
    }
}