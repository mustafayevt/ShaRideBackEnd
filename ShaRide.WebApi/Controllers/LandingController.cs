using Microsoft.AspNetCore.Mvc;

namespace ShaRide.WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LandingController : Controller
    {
        [HttpGet("")]
        [HttpGet("Home")]
        [HttpGet("Landing")]
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}